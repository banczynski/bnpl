namespace BNPL.Api.Server.src.Application.DTOs.Contract
{
    public sealed record ContractGenerationRequest(
        Guid ProposalId,
        string CustomerTaxId,
        decimal Amount,
        int Installments,
        decimal MonthlyInterestRate,
        DateTime SignedAt
    );
}
