using Core.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using Core.Context.Extensions;
using Core.Models;
using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class AnalyzeCustomerDocumentUseCase(
        IKycRepository kycRepository,
        IDocumentOcrService ocrService,
        IUserContext userContext
    )
    {
        public async Task<Result<OcrExtractionResult, string>> ExecuteAsync(Guid customerId, AnalyzeCustomerDocumentRequest request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(customerId);
            if (entity is null)
                return Result<OcrExtractionResult, string>.Fail("Customer KYC data not found.");

            var result = await ocrService.AnalyzeAsync(request.DocumentImageUrl);

            entity.DocumentNumber = result.DocumentNumber;
            entity.OcrValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.GetRequiredUserId();

            await kycRepository.UpdateAsync(entity);

            return Result<OcrExtractionResult, string>.Ok(result);
        }
    }
}
