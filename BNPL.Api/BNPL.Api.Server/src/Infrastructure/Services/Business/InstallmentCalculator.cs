using BNPL.Api.Server.src.Application.Abstractions.Business;
using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;

namespace BNPL.Api.Server.src.Infrastructure.Services.Business
{
    public class InstallmentCalculator : IInstallmentCalculator
    {
        public List<InstallmentOption> Calculate(decimal amount, int maxInstallments, decimal monthlyInterestRate)
        {
            var result = new List<InstallmentOption>();

            for (int i = 1; i <= maxInstallments; i++)
            {
                var monthlyRate = monthlyInterestRate / 100;
                var installmentValue = FinancialFormulas.PMT(monthlyRate, i, amount);
                var total = installmentValue * i;

                result.Add(new InstallmentOption
                {
                    Term = i,
                    Value = Math.Round(installmentValue, 2),
                    Total = Math.Round(total, 2)
                });
            }

            return result;
        }

        public List<InstallmentPreview> GenerateInstallments(decimal totalAmount, int quantity, int preferredDay, DateTime referenceDate)
        {
            var installments = new List<InstallmentPreview>();

            var baseAmount = Math.Floor((totalAmount / quantity) * 100) / 100;
            var remainder = totalAmount - (baseAmount * (quantity - 1));

            for (int i = 0; i < quantity; i++)
            {
                var amount = i == quantity - 1 ? remainder : baseAmount;
                var dueDate = new DateTime(referenceDate.Year, referenceDate.Month, 1)
                    .AddMonths(i + 1)
                    .AddDays(preferredDay - 1);

                if (dueDate.Day != preferredDay)
                    dueDate = new DateTime(dueDate.Year, dueDate.Month, DateTime.DaysInMonth(dueDate.Year, dueDate.Month));

                installments.Add(new InstallmentPreview
                {
                    Sequence = i + 1,
                    Amount = amount,
                    DueDate = dueDate
                });
            }

            return installments;
        }
    }
}
