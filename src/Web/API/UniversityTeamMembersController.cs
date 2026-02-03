using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.TeamMember.Commands;
using Mbrcld.Application.Features.Universities.Commands;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
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
    [Route("api/university-team-member")]
    public class UniversityTeamMembersController : BaseController
    {
        private readonly IMediator mediator;

        public UniversityTeamMembersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        [Route("instructor")]
        public async Task<ActionResult<IList<ListTeamMembersViewModel>>> ListTeamMembers()
        {
            var userId = User.GetUserId();
            var teamMembers = await this.mediator.Send(new ListTeamMembersQuery(userId));
            foreach (var teamMember in teamMembers)
            {
                teamMember.PictureUrl = Url.RouteUrl("GetTeamMemberProfilePicture", new { key = teamMember.Id });
            }
            return Ok(teamMembers);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("applicant/{moduleId}")]
        public async Task<ActionResult<IList<ListTeamMembersByModuleIdViewModel>>> ListTeamMembersByModule([FromRoute] Guid moduleId)
        {
            var teamMembers = await this.mediator.Send(new ListTeamMembersByModuleIdQuery(moduleId));
            foreach (var teamMember in teamMembers)
            {
                teamMember.PictureUrl = Url.RouteUrl("GetTeamMemberProfilePicture", new { key = teamMember.Id });
            }
            return Ok(teamMembers);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUniversityTeamMemberById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var teamMember = await this.mediator.Send(new GetTeamMemberByIdQuery(id));
            teamMember.PictureUrl = Url.RouteUrl("GetTeamMemberProfilePicture", new { key = teamMember.Id });
            return Ok(teamMember);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<ActionResult> CreateTeamMember(IFormFile file, string firstname, string lastname, string aboutmember, string email, string nationality, string residencecountry, string jobposition, string education, string linkedin, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new CreateTeamMemberCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                userId: User.GetUserId(),
                firstname: firstname,
                lastname: lastname,
                aboutmember: aboutmember,
                email: email,
                residencecountry: residencecountry,
                jobposition: jobposition,
                nationality: nationality,
                education: education,
                linkedin: linkedin
                );

                var result = await this.mediator.Send(command, cancellationToken);
                return FromResult(result);
            }
            else
            {
                var command = new CreateTeamMemberCommand(
                content: null,
                contentType: null,
                fileName: null,
                userId: User.GetUserId(),
                firstname: firstname,
                lastname: lastname,
                aboutmember: aboutmember,
                email: email,
                residencecountry: residencecountry,
                jobposition: jobposition,
                nationality: nationality,
                education: education,
                linkedin: linkedin
                );

                var result = await this.mediator.Send(command, cancellationToken);
                return FromResult(result);
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        public async Task<ActionResult> UpdateTeamMember(IFormFile file, Guid teamMemberId, string firstname, string lastname, string aboutmember, string email, string nationality, string residencecountry, string jobposition, string education, string linkedin, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new UpdateTeamMemberCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                userId: User.GetUserId(),
                teamMemberID: teamMemberId,
                firstname: firstname,
                lastname: lastname,
                aboutmember: aboutmember,
                email: email,
                residencecountry: residencecountry,
                jobposition: jobposition,
                nationality: nationality,
                education: education,
                linkedin: linkedin
                );

                var result = await this.mediator.Send(command, cancellationToken);
                return FromResult(result);
            }
            else
            {
                var command = new UpdateTeamMemberCommand(
                content: null,
                contentType: null,
                fileName: null,
                userId: User.GetUserId(),
                teamMemberID: teamMemberId,
                firstname: firstname,
                lastname: lastname,
                aboutmember: aboutmember,
                email: email,
                residencecountry: residencecountry,
                jobposition: jobposition,
                nationality: nationality,
                education: education,
                linkedin: linkedin
                );

                var result = await this.mediator.Send(command, cancellationToken);
                return FromResult(result);
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        [Route("deactivate")]
        public async Task<ActionResult<Guid>> DeactivateTeamMember(DeactivateTeamMemberCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
