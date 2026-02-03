using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("mentor-profile-pictures")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MentorProfilePictureController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;

        public MentorProfilePictureController(IUserProfilePictureService profilePictureService)
        {
            this.profilePictureService = profilePictureService;
        }

        [HttpGet("{key}", Name = "GetMentorProfilePicture")]
        public async Task<ActionResult> GetMentorProfilePicture([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var imageContents = await profilePictureService.GetMentorProfilePictureAsync(key, cancellationToken);

            if (imageContents.HasNoValue)
            {
                return NotFound();
            }

            return File(imageContents.Value, "image/jpg", key.ToString()+".jpg", false);
        }
    }
}
