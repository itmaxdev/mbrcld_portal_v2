using Mbrcld.Application.Interfaces;
using Mbrcld.Web.API;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.Controllers
{
    [Route("team-member-profile-pictures")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 86400)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TeamMemberProfilePictureController : BaseController
    {
        private readonly IUserProfilePictureService profilePictureService;

        public TeamMemberProfilePictureController(IUserProfilePictureService profilePictureService)
        {
            this.profilePictureService = profilePictureService;
        }

        [HttpGet("{key}", Name = "GetTeamMemberProfilePicture")]
        public async Task<ActionResult> GetTeamMemberProfilePicture([FromRoute] Guid key, CancellationToken cancellationToken)
        {
            var imageContents = await profilePictureService.GetUniversityTeamMemberProfilePictureAsync(key, cancellationToken);

            if (imageContents.HasNoValue)
            {
                return NotFound();
            }

            return File(imageContents.Value, "image/jpg", key.ToString()+".jpg", false);
        }
    }
}
