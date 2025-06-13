using BNPL.Api.Server.src.Application.Abstractions.External;
using Core.Persistence.Interfaces;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using BNPL.Api.Server.src.Application.DTOs.Kyc;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed record AnalyzeCustomerDocumentRequestUseCase(Guid CustomerId, AnalyzeCustomerDocumentRequest Dto);

    public sealed class AnalyzeCustomerDocumentUseCase(
        IKycRepository kycRepository,
        IDocumentOcrService ocrService,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : IUseCase<AnalyzeCustomerDocumentRequestUseCase, Result<OcrExtractionResult, Error>>
    {
        public async Task<Result<OcrExtractionResult, Error>> ExecuteAsync(AnalyzeCustomerDocumentRequestUseCase request)
        {
            var entity = await kycRepository.GetByCustomerIdAsync(request.CustomerId, unitOfWork.Transaction);
            if (entity is null)
                return Result<OcrExtractionResult, Error>.Fail(DomainErrors.Kyc.NotFound);

            var result = await ocrService.AnalyzeAsync(request.Dto.DocumentImageUrl);

            entity.DocumentNumber = result.DocumentNumber;
            entity.OcrValidated = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userContext.GetRequiredUserId();

            await kycRepository.UpdateAsync(entity, unitOfWork.Transaction);

            return Result<OcrExtractionResult, Error>.Ok(result);
        }
    }
}