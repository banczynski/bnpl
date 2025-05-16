namespace BNPL.Api.Server.src.Application.DTOs.Renegotiation
{
    public sealed record CreateRenegotiationRequest(
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        List<Guid> InstallmentIds,
        decimal NewTotalAmount,
        int NewInstallments,
        decimal MonthlyInterestRate
        );
}
