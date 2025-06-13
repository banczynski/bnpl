namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public sealed class BusinessRulesSettings
    {
        public int MaxProposalCancellationDays { get; set; } = 7;

        public int MaxItemReturnDays { get; set; } = 7;
    }
}
