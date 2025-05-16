using BNPL.Api.Server.src.Application.DTOs.Invoice;

namespace BNPL.Api.Server.src.Application.Services.External
{
    public interface IPaymentLinkService
    {
        Task<Uri> GenerateAsync(PaymentLinkRequest request);
    }
}
