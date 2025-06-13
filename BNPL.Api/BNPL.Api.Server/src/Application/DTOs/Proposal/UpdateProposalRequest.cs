namespace BNPL.Api.Server.src.Application.DTOs.Proposal
{
    public sealed record UpdateProposalRequest(
        decimal RequestedAmount,
        decimal TotalWithCharges,
        int Installments,
        decimal MonthlyInterestRate,
        int PreferredDueDay
    );
}
