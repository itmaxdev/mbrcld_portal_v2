using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("attached-pictures")]
    //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AttachedPictureController : BaseController
    {
        private readonly IAttachedPictureService attachedPictureService;

        public AttachedPictureController(IAttachedPictureService attachedPictureService)
        {
            this.attachedPictureService = attachedPictureService;
        }

        [HttpGet("{key}", Name = "GetAttachedPicture")]
        public async Task<ActionResult> GetAttachedPicture([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var imageContents = await attachedPictureService.GetAttachedPictureAsync(key, cancellationToken);

            if (imageContents.HasNoValue)
            {
                return NotFound();
            }

            return File(imageContents.Value, "image/png", "picture.png", false);
        }
    }
}
