using BNPL.Api.Server.src.Application.DTOs.Installment;

namespace BNPL.Api.Server.src.Application.Abstractions.Business
{
    public interface IChargesCalculatorService
    {
        InstallmentChargesResult Calculate(InstallmentChargesInput input);
    }
}
