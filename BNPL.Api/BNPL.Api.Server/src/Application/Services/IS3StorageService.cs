namespace BNPL.Api.Server.src.Application.Services
{
    public interface IS3StorageService
    {
        string GeneratePresignedUploadUrl(string fileName, string folder, TimeSpan expiration);
    }
}
