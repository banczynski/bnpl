using BNPL.Api.Server.src.Application.Repositories;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.ProposalItem
{
    public sealed class MarkProposalItemAsReturnedUseCase(
        IProposalItemRepository repository
    )
    {
        public async Task<ServiceResult<string>> ExecuteAsync(Guid proposalId, Guid productId, string reason)
        {
            await repository.MarkAsReturnedAsync(proposalId, productId, reason);
            return new ServiceResult<string>("Item marked as returned.");
        }
    }
}
