using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Services;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class ChargesCalculatorService : IChargesCalculatorService
    {
        // TODO
        public InstallmentChargesResult Calculate(InstallmentChargesInput input)
        {
            var daysLate = (input.PaymentDate.Date - input.DueDate.Date).Days;
            if (daysLate <= 0)
            {
                return new InstallmentChargesResult(
                    DaysLate: 0,
                    FixedCharges: 0m,
                    InterestAmount: 0m,
                    TotalWithCharges: input.OriginalAmount
                );
            }

            var fixedCharges = input.OriginalAmount * input.FixedChargesRate;
            var interest = input.OriginalAmount * input.DailyInterestRate * daysLate;
            var total = input.OriginalAmount + fixedCharges + interest;

            return new InstallmentChargesResult(
                DaysLate: daysLate,
                FixedCharges: decimal.Round(fixedCharges, 2),
                InterestAmount: decimal.Round(interest, 2),
                TotalWithCharges: decimal.Round(total, 2)
            );
        }
    }
}
