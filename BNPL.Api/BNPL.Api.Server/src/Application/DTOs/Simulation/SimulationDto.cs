namespace BNPL.Api.Server.src.Application.DTOs.Simulation
{
    public sealed record SimulationDto(
        Guid Code,
        Guid PartnerId,
        Guid AffiliateId,
        string CustomerTaxId,
        decimal RequestedAmount,
        decimal ApprovedLimit,
        int MaxInstallments,
        decimal MonthlyInterestRate,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
