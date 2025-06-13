using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.Abstractions.Storage;
using BNPL.Api.Server.src.Application.DTOs.Contract;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GenerateFinalContractUseCase(
        IProposalRepository proposalRepository,
        IPdfContractService pdfContractService
    )
    {
        public async Task<Result<Uri, Error>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await proposalRepository.GetByIdAsync(proposalId);
            if (proposal is null)
                return Result<Uri, Error>.Fail(DomainErrors.Proposal.NotFound);

            if (proposal.Status != ProposalStatus.Active)
                return Result<Uri, Error>.Fail(DomainErrors.Proposal.MustBeFormalized);

            var url = await pdfContractService.GenerateFinalDocumentAsync(new ContractGenerationRequest(
                ProposalId: proposal.Code,
                CustomerTaxId: proposal.CustomerTaxId,
                Amount: proposal.TotalWithCharges,
                Installments: proposal.Term,
                MonthlyInterestRate: proposal.MonthlyInterestRate,
                SignedAt: proposal.UpdatedAt
            ));

            return Result<Uri, Error>.Ok(url);
        }
    }
}