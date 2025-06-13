using BNPL.Api.Server.src.Application.Abstractions.External;
using BNPL.Api.Server.src.Application.DTOs.Invoice;

namespace BNPL.Api.Server.src.Infrastructure.Services.External
{
    // TODO
    public sealed class FakePaymentLinkService : IPaymentLinkService
    {
        public Task<Uri> GenerateAsync(PaymentLinkRequest request)
        {
            var link = $"https://fakepay.example.com/pay/{request.InvoiceId}";
            return Task.FromResult(new Uri(link));
        }
    }
}
