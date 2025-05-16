using BNPL.Api.Server.src.Application.DTOs.Contract;

namespace BNPL.Api.Server.src.Application.Services.External
{
    public interface IPdfContractService
    {
        Task<Uri> GenerateFinalDocumentAsync(ContractGenerationRequest request);
    }
}
