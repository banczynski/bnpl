using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Customer;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Domain.Entities;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;
using Core.Persistence.Interfaces;

namespace BNPL.Api.Server.src.Application.UseCases.Customer
{
    public sealed record CreateCustomerRequestUseCase(Guid AffiliateId, CreateCustomerRequest Dto);

    public sealed class CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        ICustomerBillingPreferencesRepository customerBillingPreferencesRepository,
        IAffiliateRepository affiliateRepository,
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<CreateCustomerRequestUseCase, Result<CreateCustomerResponse, Error>>
    {
        public async Task<Result<CreateCustomerResponse, Error>> ExecuteAsync(CreateCustomerRequestUseCase useCaseRequest)
        {
            var (affiliateId, request) = useCaseRequest;

            var partnerId = await affiliateRepository.GetPartnerIdByAffiliateIdAsync(affiliateId, unitOfWork.Transaction);
            if (partnerId.GetValueOrDefault() == Guid.Empty)
                return Result<CreateCustomerResponse, Error>.Fail(DomainErrors.Affiliate.NotFound);

            var existingCustomers = await customerRepository.GetByTaxIdAsync(request.TaxId, unitOfWork.Transaction);
            if (existingCustomers is not null && existingCustomers.Any(c => c?.PartnerId == partnerId || c?.AffiliateId == affiliateId))
                return Result<CreateCustomerResponse, Error>.Fail(DomainErrors.Customer.AlreadyExists);

            var entity = request.ToEntity(partnerId.Value, affiliateId, userContext.GetRequiredUserId());

            await customerRepository.InsertAsync(entity, unitOfWork.Transaction);

            var billingPreferences = new CustomerBillingPreferences
            {
                Code = Guid.NewGuid(),
                CustomerId = entity.Code,
                PartnerId = partnerId.Value,
                AffiliateId = affiliateId,
                CustomerTaxId = entity.TaxId,
                InvoiceDueDay = 10,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userContext.GetRequiredUserId(),
                UpdatedBy = userContext.GetRequiredUserId(),
                ConsolidatedInvoiceEnabled = false,
                IsActive = true
            };

            await customerBillingPreferencesRepository.InsertAsync(billingPreferences, unitOfWork.Transaction);

            if (request.SkipKyc)
            {
                var existingKyc = await kycRepository.GetByCustomerIdAsync(entity.Code, unitOfWork.Transaction);
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
                    await kycRepository.InsertAsync(kyc, unitOfWork.Transaction);
                }
            }

            return Result<CreateCustomerResponse, Error>.Ok(new CreateCustomerResponse(entity.Code));
        }
    }
}