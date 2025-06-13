namespace BNPL.Api.Server.src.Application.Abstractions.External
{
    public interface IFaceMatchService
    {
        Task<bool> ValidateAsync(Uri documentImageUrl, Uri selfieImageUrl);
    }
}
