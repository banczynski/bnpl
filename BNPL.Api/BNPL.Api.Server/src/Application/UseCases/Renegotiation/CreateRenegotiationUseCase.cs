using BNPL.Api.Server.src.Application.Context.Interfaces;
using BNPL.Api.Server.src.Application.DTOs.Renegotiation;
using BNPL.Api.Server.src.Application.Mappers;
using BNPL.Api.Server.src.Application.Repositories;
using BNPL.Api.Server.src.Domain.Enums;
using Core.Models;

namespace BNPL.Api.Server.src.Application.UseCases.Renegotiation
{
    public sealed class CreateRenegotiationUseCase(
        IInstallmentRepository installmentRepository,
        IRenegotiationRepository renegotiationRepository,
        IUserContext userContext
    )
    {
        public async Task<ServiceResult<Guid>> ExecuteAsync(CreateRenegotiationRequest request)
        {
            var now = DateTime.UtcNow;

            var installments = (await installmentRepository.GetPendingByIdsAsync(request.InstallmentIds)).ToList();

            if (installments.Count != request.InstallmentIds.Count)
                throw new InvalidOperationException("Some installments were not found or are not eligible for renegotiation.");

            if (installments.Select(i => i.CustomerTaxId).Distinct().Count() > 1)
                throw new InvalidOperationException("All installments must belong to the same customer.");

            var originalAmount = installments.Sum(i => i.Amount);
            var invoiceIds = installments
                .Where(i => i.InvoiceId.HasValue)
                .Select(i => i.InvoiceId!.Value)
                .Distinct()
                .ToList();

            var renegotiation = request.ToEntity(
                id: Guid.NewGuid(),
                now: now,
                user: userContext.UserId,
                invoiceIds: invoiceIds,
                originalTotal: originalAmount
                );

            await renegotiationRepository.InsertAsync(renegotiation);

            return new ServiceResult<Guid>(renegotiation.Id, ["Renegotiation created successfully."]);
        }
    }
}
