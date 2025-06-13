using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class CalculateInstallmentChargesUseCase(
        IInstallmentRepository installmentRepository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService
    )
    {
        public async Task<Result<InstallmentChargesResult, Error>> ExecuteAsync(Guid installmentId, DateTime? paymentDate = null)
        {
            var entity = await installmentRepository.GetByIdAsync(installmentId);
            if (entity is null)
                return Result<InstallmentChargesResult, Error>.Fail(DomainErrors.Installment.NotFound);

            var config = await configService.GetEffectiveConfigAsync(entity.PartnerId, entity.AffiliateId);

            var input = new InstallmentChargesInput(
                OriginalAmount: entity.Amount,
                DueDate: entity.DueDate,
                PaymentDate: paymentDate?.Date ?? DateTime.UtcNow.Date,
                DailyInterestRate: config.InterestRate,
                FixedChargesRate: config.ChargesRate
            );

            var result = chargesCalculator.Calculate(input);

            return Result<InstallmentChargesResult, Error>.Ok(result);
        }
    }
}