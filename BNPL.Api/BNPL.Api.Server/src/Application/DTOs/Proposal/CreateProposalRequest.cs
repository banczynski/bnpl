namespace BNPL.Api.Server.src.Application.DTOs.Proposal
{
    public sealed record CreateProposalRequest(
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        Guid SimulationId,
        decimal RequestedAmount,
        decimal ApprovedAmount,
        int Installments,
        decimal MonthlyInterestRate
    );
}
