namespace BNPL.Api.Server.src.Domain.Enums
{
    public enum ProposalStatus
    {
        Created = 0,
        AwaitingKyc = 1,
        AwaitingSignature = 2, 
        Signed = 3,
        Formalized = 4, 
        Disbursed = 5,
        Cancelled = 6,
        Rejected = 7 ,
        Finalized = 8
    }
}
