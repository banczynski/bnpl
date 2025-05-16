using BNPL.Api.Server.src.Domain.Enums;

namespace BNPL.Api.Server.src.Application.DTOs.Proposal
{
    public sealed record ProposalDto(
        Guid Id,
        Guid PartnerId,
        Guid AffiliateId,
        Guid CustomerId,
        string CustomerTaxId,
        Guid SimulationId,
        decimal RequestedAmount,
        decimal ApprovedAmount,
        int Installments,
        decimal MonthlyInterestRate,
        ProposalStatus Status,
        bool IsActive,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        string CreatedBy,
        string UpdatedBy
    );
}
