using BNPL.Api.Server.src.Application.Services;
using BNPL.Api.Server.src.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BNPL.Api.Server.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class FileUploadController(IS3StorageService s3StorageService) : ControllerBase
    {
        [HttpGet("url")]
        public ActionResult<string> GetPresignedUrl([FromQuery] string fileName, [FromQuery] DocumentType documentType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("fileName is required.");

            var folder = documentType.ToString().ToLowerInvariant(); 

            var url = s3StorageService.GeneratePresignedUploadUrl(fileName, folder, TimeSpan.FromMinutes(10));
            return Ok(url);
        }
    }
}
