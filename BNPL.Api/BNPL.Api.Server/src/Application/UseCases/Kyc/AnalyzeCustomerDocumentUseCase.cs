using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Application.Services.External;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class AnalyzeCustomerDocumentUseCase(
        IKycRepository kycRepository,
        IDocumentOcrService ocrService,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<OcrExtractionResult>> ExecuteAsync(Guid customerId, AnalyzeCustomerDocumentRequest request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId)
                ?? throw new InvalidOperationException("Customer KYC data not found.");

            // TODO
            var result = await ocrService.AnalyzeAsync(request.DocumentImageUrl);

            entity.DocumentNumber = result.DocumentNumber;
            entity.OcrValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.UserId;

            await kycRepository.UpdateAsync(entity);

            return new ServiceResult<OcrExtractionResult>(result, ["OCR analysis completed successfully."]);
        }
    }
}
