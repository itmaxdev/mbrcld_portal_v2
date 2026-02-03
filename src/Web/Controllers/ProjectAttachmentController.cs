using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("project-attachment")]
    //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProjectAttachmentController : BaseController
    {
        private readonly IAttachedPictureService attachedPictureService;

        public ProjectAttachmentController(IAttachedPictureService attachedPictureService)
        {
            this.attachedPictureService = attachedPictureService;
        }

        [HttpGet("{url}", Name = "GetAttachment")]
        public async Task<ActionResult> GetProjectAttachment([FromRoute] string url, CancellationToken cancellationToken)
        {
            var contents = await attachedPictureService.GetAttachmentAsync(url, cancellationToken);
            if (contents.HasNoValue)
            {
                return NotFound();
            }
            var filename = Path.GetFileName(url);
            var contentType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(filename, out contentType);
            return File(contents.Value, contentType, filename);
        }
    }
}
