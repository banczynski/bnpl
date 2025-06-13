namespace BNPL.Api.Server.src.Application.Abstractions.Storage
{
    public interface IS3StorageService
    {
        string GeneratePresignedUploadUrl(string fileName, string folder, TimeSpan expiration);
    }
}
