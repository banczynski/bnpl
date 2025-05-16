namespace BNPL.Api.Server.src.Application.DTOs.Kyc
{
    public sealed record OcrExtractionResult(
        string DocumentNumber,
        string Name,
        string TaxId,
        DateTime? BirthDate,
        DateTime? ExpirationDate
    );
}
