namespace BNPL.Api.Server.src.Application.DTOs.Kyc
{
    public sealed record AnalyzeCustomerDocumentRequest(
        Uri DocumentImageUrl
    );
}
