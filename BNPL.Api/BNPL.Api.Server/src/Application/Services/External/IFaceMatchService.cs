namespace BNPL.Api.Server.src.Application.Services.External
{
    public interface IFaceMatchService
    {
        Task<bool> ValidateAsync(Uri documentImageUrl, Uri selfieImageUrl);
    }
}
