using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;

namespace BNPL.Api.Server.src.Application.Abstractions.Business
{
    public interface IInstallmentCalculator
    {
        List<InstallmentOption> Calculate(decimal amount, int maxInstallments, decimal monthlyInterestRate);
        List<InstallmentPreview> GenerateInstallments(decimal totalAmount, int quantity, int preferredDay, DateTime referenceDate);
    }

}
