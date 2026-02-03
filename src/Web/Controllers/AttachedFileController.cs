using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("attached-file")]
    //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AttachedFileController : BaseController
    {
        private readonly IAttachedPictureService attachedPictureService;

        public AttachedFileController(IAttachedPictureService attachedPictureService)
        {
            this.attachedPictureService = attachedPictureService;
        }

        [HttpGet("{key}", Name = "GetAttachedFile")]
        public async Task<ActionResult> GetAttachedFile([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var fileDetails = await attachedPictureService.GetAttachedFileTypeAsync(key, cancellationToken);
            var fileContents = await attachedPictureService.GetAttachedFileAsync(key, cancellationToken);

            if (fileContents.HasNoValue)
            {
                return NotFound();
            }

            var fileName = fileDetails[0];
            var fileType = fileDetails[1];
            return File(fileContents.Value, fileType, fileName, false);
        }
    }
}
