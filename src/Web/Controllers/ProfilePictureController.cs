using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("profile-pictures")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProfilePictureController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;

        public ProfilePictureController(IUserProfilePictureService profilePictureService)
        {
            this.profilePictureService = profilePictureService;
        }

        [HttpGet("{key}", Name = "GetProfilePicture")]
        public async Task<ActionResult> GetProfilePicture([FromRoute] string key, CancellationToken cancellationToken)
        {
            var imageContents = await profilePictureService.GetProfilePictureAsync(key, cancellationToken);

            if (imageContents.HasNoValue)
            {
                return NotFound();
            }

            return File(imageContents.Value, "image/jpg", "profile-picture.jpg", false);
        }
    }
}
