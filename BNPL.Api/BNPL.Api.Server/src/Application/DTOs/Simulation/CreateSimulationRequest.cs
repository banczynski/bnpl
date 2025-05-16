namespace BNPL.Api.Server.src.Application.DTOs.Simulation
{
    public sealed record CreateSimulationRequest(
        Guid PartnerId,
        Guid AffiliateId,
        string CustomerTaxId,
        decimal RequestedAmount
    );
}
