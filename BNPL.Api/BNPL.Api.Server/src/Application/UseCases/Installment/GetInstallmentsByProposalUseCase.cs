using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Installment;
using BNPL.Api.Server.src.Application.Mappers;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Installment
{
    public sealed class GetInstallmentsByProposalUseCase(IInstallmentRepository installmentRepository)
    {
        public async Task<Result<IEnumerable<InstallmentDto>, Error>> ExecuteAsync(Guid proposalId)
        {
            var list = await installmentRepository.GetByProposalIdAsync(proposalId);
            return Result<IEnumerable<InstallmentDto>, Error>.Ok(list.ToDtoList());
        }
    }
}