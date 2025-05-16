using Amazon.S3;
using Amazon.S3.Model;
using BNPL.Api.Server.src.Application.Services;

namespace BNPL.Api.Server.src.Infrastructure.Services
{
    public sealed class S3StorageService(IAmazonS3 s3Client, IConfiguration configuration) : IS3StorageService
    {
        public string GeneratePresignedUploadUrl(string fileName, string folder, TimeSpan expiration)
        {
            var bucketName = configuration["AWS:S3:BucketName"]
                ?? throw new InvalidOperationException("S3 bucket name not configured.");

            var key = $"{folder}/{fileName}";

            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.Add(expiration)
            };

            return s3Client.GetPreSignedURL(request);
        }
    }
}
