using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class CalculateInstallmentChargesUseCase(
        IInstallmentRepository repository,
        IChargesCalculatorService chargesCalculator,
        IFinancialChargesConfigurationService configService
    )
    {
        public async Task<ServiceResult<InstallmentChargesResult>> ExecuteAsync(Guid installmentId, DateTime? paymentDate = null)
        {
            var entity = await repository.GetByIdAsync(installmentId)
                ?? throw new InvalidOperationException("Installment not found.");

            var config = await configService.GetEffectiveConfigAsync(entity.PartnerId, entity.AffiliateId);

            var input = new InstallmentChargesInput(
                OriginalAmount: entity.Amount,
                DueDate: entity.DueDate,
                PaymentDate: paymentDate?.Date ?? DateTime.UtcNow.Date,
                DailyInterestRate: config.InterestRate,
                FixedChargesRate: config.ChargesRate
            );

            var result = chargesCalculator.Calculate(input);

            return new ServiceResult<InstallmentChargesResult>(result);
        }
    }
}
