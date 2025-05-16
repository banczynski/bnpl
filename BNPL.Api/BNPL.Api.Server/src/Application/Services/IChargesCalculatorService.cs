using BNPL.Api.Server.src.Application.DTOs.Installment;

namespace BNPL.Api.Server.src.Application.Services
{
    public interface IChargesCalculatorService
    {
        InstallmentChargesResult Calculate(InstallmentChargesInput input);
    }
}
