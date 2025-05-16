using BNPL.Api.Server.src.Application.DTOs.Contract;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Proposal
{
    public sealed class GenerateFinalContractUseCase(
        IProposalRepository repository,
        IPdfContractService pdfContractService
    )
    {
        public async Task<ServiceResult<Uri>> ExecuteAsync(Guid proposalId)
        {
            var proposal = await repository.GetByIdAsync(proposalId)
                ?? throw new InvalidOperationException("Proposal not found.");

            if (proposal.Status != ProposalStatus.Signed)
                throw new InvalidOperationException("Proposal must be signed to generate the contract.");

            // TODO
            var url = await pdfContractService.GenerateFinalDocumentAsync(new ContractGenerationRequest(
                ProposalId: proposal.Id,
                CustomerTaxId: proposal.CustomerTaxId,
                Amount: proposal.ApprovedAmount,
                Installments: proposal.Installments,
                MonthlyInterestRate: proposal.MonthlyInterestRate,
                SignedAt: proposal.UpdatedAt
            ));

            return new ServiceResult<Uri>(url, ["Final contract generated successfully."]);
        }
    }
}
