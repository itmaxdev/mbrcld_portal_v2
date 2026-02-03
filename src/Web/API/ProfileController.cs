using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Application.Features.Users.Queries;
using Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/profile")]
    public class ProfileController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IUserProfilePictureService profilePictureService;

        public ProfileController(IMediator mediator, IUserProfilePictureService profilePictureService)
        {
            this.mediator = mediator;
            this.profilePictureService = profilePictureService;
        }

        [Authorize]
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<GetUserProfileViewModel>> GetProfile()
        {
            var userId = User.GetUserId();

            var profile = await this.mediator.Send(new GetUserProfileQuery(userId));
            if (profile.HasNoValue)
            {
                return NotFound();
            }

            profile.Value.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = profile.Value.ProfilePictureUrl });

            return Ok(profile.Value);
        }

        [HttpGet("{phoneNumber}")]
        public async Task<ActionResult<GetUserByPhoneNumberViewModel>> GetProfileByPhoneNumber([FromRoute] string phoneNumber)
        {

            var profile = await this.mediator.Send(new GetUserByPhoneNumber(phoneNumber));
            if (profile.HasNoValue)
            {
                return null;
            }
            return Ok(profile.Value);
        }

        [Authorize]
        [HttpPut("is-active-member", Name = "SetIsActiveMember")]
        public async Task<ActionResult> SetIsActiveMember(SetIsActiveMemberCommand command, CancellationToken cancellationToken)
        {
            command.UserId = User.GetUserId();
            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet]
        [Route("profile-completion")]
        public async Task<ActionResult<GetUserProfileCompletionViewModel>> GetProfileCompletion()
        {
            var result = await this.mediator.Send(new GetUserProfileCompletionQuery(User.GetUserId()));
            var contactAsync = await this.mediator.Send(new GetUserProfileQuery(User.GetUserId()));
            var profileUpdatedOn = contactAsync.ValueOrDefault.ProfileUpdateOn;

            bool requiresUpdate =
                profileUpdatedOn == null ||
                profileUpdatedOn < DateTime.Now.AddMonths(-1);
            result.RequiresUpdate = requiresUpdate;
            return Ok(result);
        } 
        
        [Authorize]
        [HttpPost]
        [Route("reset-last-update")]
        public async Task<IActionResult> ResetLastUpdate()
        {
            // Update the "do_LastProfileUpdateOn" field to today's date
            var contactAsync = await this.mediator.Send(new GetUserProfileQuery(User.GetUserId()));
            var contact = contactAsync.ValueOrDefault;
            if (contact != null)
            {
                var command = new EditUserProfileUpdateOnCommand();
                command.Id = contact.Id;
                command.ProfileUpdateOn = DateTime.Now;
                var result = await this.mediator.Send(command);
                return FromResult(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("general-information")]
        public async Task<ActionResult> EditGeneralInformation(EditUserGeneralInformationCommand command)
        {
            var userId = User.GetUserId();
            command.Id = userId;
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost]
        [Route("contact-details")]
        public async Task<ActionResult> EditContactDetails(EditUserContactDetailsCommand command)
        {
            var userId = User.GetUserId();
            command.Id = userId;
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost]
        [Route("identity-details")]
        public async Task<ActionResult> EditUserIdentityDetails(EditUserIdentityDetailsCommand command)
        {
            var userId = User.GetUserId();
            command.Id = userId;
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize]
        [HttpGet]
        [EnableQuery]
        [Route("learning-preferences")]
        public async Task<ActionResult<GetUserLearningPreferencesViewModel>> GetLearningPreferences()
        {
            var userId = User.GetUserId();

            var user = await this.mediator.Send(new GetUserLearningPreferencesQuery(userId));
            if (user.HasNoValue)
            {
                return NotFound();
            }

            return Ok(user.Value);
        }

        [Authorize]
        [HttpPost]
        [Route("learning-preferences")]
        public async Task<ActionResult> EditLearningPreferences(EditUserLearningPreferencesCommand command)
        {
            command.UserId = User.GetUserId();
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize]
        [HttpPost]
        [Route("profile-picture")]
        public async Task<ActionResult> EditProfilePicture(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            {
                return BadRequest();
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var content = ms.ToArray();

            var userId = User.GetUserId();

            await this.profilePictureService.ChangeProfilePictureAsync(userId, content, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        [Route("profile-picture")]
        public async Task<ActionResult> RemoveProfilePicture(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await this.profilePictureService.RemoveProfilePictureAsync(userId, cancellationToken);
            return FromResult(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        [Route("about-instructor")]
        public async Task<ActionResult> EditInstructorAbout(string about)
        {
            var userId = User.GetUserId();
            var command = new EditInstructorAboutCommand();
            command.Id = userId;
            command.AboutInstructor = about;

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        [Route("about-university")]
        public async Task<ActionResult> EditInstructorUniversityAbout(string aboutuniversity)
        {
            var userId = User.GetUserId();
            var command = new EditInstructorUniversityOverviewCommand();
            command.Id = userId;
            command.AboutUniversity = aboutuniversity;

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [Authorize(Roles = "Direct Manager")]
        [HttpGet]
        [Route("directmanager-applicants")]
        public async Task<ActionResult<IList<ListDirectManagerApplicantsViewModel>>> ListDirectManagerApplicants()
        {
            var applicants = await this.mediator.Send(new ListDirectManagerApplicantsQuery(User.GetUserId()));
            foreach (var applicant in applicants)
            {
                applicant.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = applicant.ProfilePictureUrl });
            }
            return Ok(applicants);
        }

        [Authorize]
        [HttpGet("search-alumni/{text}", Name = "SearchAlumni")]
        public async Task<ActionResult<IList<SearchAlumniViewModel>>> SearchAlumni([FromRoute] string text)
        {
            var alumnies = await this.mediator.Send(new SearchAlumniQuery(text));
            foreach (var alumni in alumnies)
            {
                alumni.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = alumni.ProfilePictureUrl });
            }
            return Ok(alumnies);
        }

        [Authorize]
        [HttpGet("search-alumni-criteria", Name = "SearchAlumniCriteria")]
        public async Task<ActionResult<IList<SearchAlumniViewModel>>> SearchAlumniByCriteria(Guid? programId = null, Guid? sectorId = null, int? year = null)
        {
            var userId = User.GetUserId();
            var alumnies = await this.mediator.Send(new SearchAlumniCriteriaQuery(programId, sectorId, year, userId));
            foreach (var alumni in alumnies)
            {
                alumni.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = alumni.ProfilePictureUrl });
            }
            return Ok(alumnies);
        }

        [Authorize]
        [HttpGet("users-for-chat", Name = "GetUsersForChat")]
        public async Task<ActionResult<IList<ListAlumniUsersForChatViewModel>>> GetUsersForChat()
        {
            var userId = User.GetUserId();
            var alumnies = await this.mediator.Send(new ListAlumniUsersForChatQuery(userId));
            foreach (var alumni in alumnies)
            {
                alumni.ProfilePictureUrl = Url.RouteUrl("GetProfilePicture", new { key = alumni.ProfilePictureUrl });
            }
            return Ok(alumnies);
        }

        [Authorize]
        [HttpGet("user-disclaimer", Name = "GetUserDisclaimer")]
        public async Task<ActionResult<IList<GetUserDisclaimerViewModel>>> GetUserDisclaimer(int disclaimerType)
        {
            var userId = User.GetUserId();
            var user = await this.mediator.Send(new GetUserDisclaimerQuery(userId, disclaimerType));
            return Ok(user.Value);
        }

        [Authorize]
        [HttpPut]
        [Route("user-disclaimer")]
        public async Task<ActionResult> SetUserDisclaimer(int disclaimerType)
        {
            var userId = User.GetUserId();
            var command = new EditUserDisclaimerCommand();
            command.Id = userId;
            command.DisclaimerType = disclaimerType;

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }
    }
}
