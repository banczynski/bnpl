namespace BNPL.Api.Server.src.Application.DTOs.Signature
{
    public sealed record SignatureRequest(
        Guid ProposalId,
        string CustomerName,
        string CustomerTaxId,
        decimal ApprovedAmount,
        int Installments
    );
}
