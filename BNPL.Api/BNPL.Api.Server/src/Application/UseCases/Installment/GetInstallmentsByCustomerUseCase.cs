using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class GetInstallmentsByCustomerUseCase(IInstallmentRepository repository)
    {
        public async Task<ServiceResult<IEnumerable<InstallmentDto>>> ExecuteAsync(Guid customerId)
        {
            var list = await repository.GetPendingByCustomerIdAsync(customerId);
            return new ServiceResult<IEnumerable<InstallmentDto>>(list.Select(i => i.ToDto()));
        }
    }
}
