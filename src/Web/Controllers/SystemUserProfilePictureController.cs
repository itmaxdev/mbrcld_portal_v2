using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("system-user-profile-pictures")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SystemUserProfilePictureController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;

        public SystemUserProfilePictureController(IUserProfilePictureService profilePictureService)
        {
            this.profilePictureService = profilePictureService;
        }

        [HttpGet("{key}", Name = "GetSystemUserProfilePicture")]
        public async Task<ActionResult> GetSystemUserProfilePicture([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var imageContents = await profilePictureService.GetSystemUserProfilePictureAsync(key, cancellationToken);

            if (imageContents.HasNoValue)
            {
                return NotFound();
            }

            return File(imageContents.Value, "image/jpg", key.ToString()+".jpg", false);
        }
    }
}
