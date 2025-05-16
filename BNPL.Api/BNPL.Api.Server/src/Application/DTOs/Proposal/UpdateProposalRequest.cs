using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Proposal
{
    public sealed record UpdateProposalRequest(
        decimal RequestedAmount,
        decimal ApprovedAmount,
        int Installments,
        decimal MonthlyInterestRate
    );
}
