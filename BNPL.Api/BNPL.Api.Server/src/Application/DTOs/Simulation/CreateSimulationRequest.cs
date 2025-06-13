namespace BNPL.Api.Server.src.Application.DTOs.Simulation
{
    public sealed record CreateSimulationRequest(
        string CustomerTaxId,
        decimal RequestedAmount
    );
}
