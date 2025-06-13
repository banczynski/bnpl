using BNPL.Api.Server.src.Domain.Entities;

namespace BNPL.Api.Server.src.Application.Abstractions.Business
{
    public interface IFinancialChargesConfigurationService
    {
        Task<FinancialChargesConfiguration> GetEffectiveConfigAsync(Guid partnerId, Guid? affiliateId);
    }
}
