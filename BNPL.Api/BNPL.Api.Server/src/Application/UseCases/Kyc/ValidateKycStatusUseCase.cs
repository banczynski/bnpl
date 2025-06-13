using BNPL.Api.Server.src.Application.Abstractions.Persistence;
using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Context.Extensions;
using Core.Context.Interfaces;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Kyc
{
    public sealed class ValidateKycStatusUseCase(
        IKycRepository kycRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        public async Task<Result<string, string[]>> ExecuteAsync(Guid customerId)
        {
            using var scope = unitOfWork;

            try
            {
                scope.Begin();

                var entity = await kycRepository.GetByCustomerIdAsync(customerId, scope.Transaction);
                if (entity is null)
                    return Result<string, string[]>.Fail(["KYC data not found."]);

                if (entity.OcrValidated && entity.FaceMatchValidated)
                {
                    entity.MarkAsValidated(DateTime.UtcNow, userContext.GetRequiredUserId());
                    await kycRepository.UpdateAsync(entity, scope.Transaction);

                    scope.Commit();
                    return Result<string, string[]>.Ok("KYC status set to Validated.");
                }

                return Result<string, string[]>.Fail([
                    $"KYC not fully validated.",
                    $"OCR: {entity.OcrValidated}",
                    $"FaceMatch: {entity.FaceMatchValidated}"
                ]);
            }
            catch
            {
                scope.Rollback();
                throw;
            }
        }
    }
}
