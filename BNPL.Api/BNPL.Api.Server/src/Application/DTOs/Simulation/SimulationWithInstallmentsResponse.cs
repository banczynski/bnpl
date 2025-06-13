using BNPL.Api.Server.src.Application.DTOs.FinancialCharges;

namespace BNPL.Api.Server.src.Application.DTOs.Simulation
{
    public class SimulationWithInstallmentsResponse
    {
        public SimulationResponse Simulation { get; set; } = default!;
        public List<InstallmentOption> Installments { get; set; } = default!;
    }
}
