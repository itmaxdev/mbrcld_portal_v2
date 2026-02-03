using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("attached-cv")]
    //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AttachedCVController : BaseController
    {
        private readonly IAttachedPictureService attachedPictureService;

        public AttachedCVController(IAttachedPictureService attachedPictureService)
        {
            this.attachedPictureService = attachedPictureService;
        }

        [HttpGet("{key}", Name = "GetAttachedCV")]
        public async Task<ActionResult> GetAttachedFile([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var fileDetails = await attachedPictureService.GetAttachedCVTypeAsync(key, cancellationToken);
            var fileContents = await attachedPictureService.GetAttachedCVAsync(key, cancellationToken);

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
