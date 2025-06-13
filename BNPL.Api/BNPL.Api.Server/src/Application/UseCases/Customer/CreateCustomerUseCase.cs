using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed class CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        ICustomerBillingPreferencesRepository customerBillingPreferencesRepository,
        IAffiliateRepository affiliateRepository,
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<CreateCustomerResponse, string>> ExecuteAsync(Guid affiliateId, CreateCustomerRequest request)
        {
            using var scope = unitOfWork;

            try
            {
                var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(affiliateId);
                if (partnerId.GetValueOrDefault() == Guid.Empty)
                    return Result<CreateCustomerResponse, string>.Fail("Affiliate not found.");

                var existingCustomers = await customerRepository.GetByTaxIdAsync(request.TaxId);
                if (existingCustomers is not null && existingCustomers.Any(c => c?.PartnerId == partnerId || c?.AffiliateId == affiliateId))
                    return Result<CreateCustomerResponse, string>.Fail("A customer with the provided Tax ID already exists for this partner or affiliate.");

                scope.Begin();

                var entity = request.ToEntity(partnerId.Value, affiliateId, userContext.GetRequiredUserId());

                await customerRepository.InsertAsync(entity, scope.Transaction);

                var billingPreferences = new CustomerBillingPreferences
                {
                    CustomerId = entity.Code,
                    AffiliateId = affiliateId,
                    InvoiceDueDay = 10,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = userContext.GetRequiredUserId(),
                    UpdatedBy = userContext.GetRequiredUserId(),
                    ConsolidatedInvoiceEnabled = false
                };

                await customerBillingPreferencesRepository.InsertAsync(billingPreferences, scope.Transaction);

                if (request.SkipKyc)
                {
                    var existingKyc = await kycRepository.GetByCustomerIdAsync(entity.Code, scope.Transaction);
                    if (existingKyc is null)
                    {
                        var kyc = new Domain.Entities.Kyc
                        {
                            Code = Guid.NewGuid(),
                            CustomerId = entity.Code,
                            Status = KycStatus.Validated,
                            OcrValidated = true,
                            FaceMatchValidated = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedBy = userContext.GetRequiredUserId(),
                            UpdatedBy = userContext.GetRequiredUserId(),
                            IsActive = true
                        };

                        await kycRepository.InsertAsync(kyc, scope.Transaction);
                    }
                }

                scope.Commit();

                return Result<CreateCustomerResponse, string>.Ok(new CreateCustomerResponse(entity.Code));
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
