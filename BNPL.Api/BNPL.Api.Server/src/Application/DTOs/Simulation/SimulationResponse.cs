namespace BNPL.Api.Server.src.Application.DTOs.Simulation
{
    public sealed record SimulationResponse(
        Guid Id,
        decimal ApprovedAmount,
        int MaxInstallments,
        decimal MonthlyInterestRate
    );
}
