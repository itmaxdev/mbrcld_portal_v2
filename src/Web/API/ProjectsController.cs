using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.Projects.Commands;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Application.Interfaces;
using Mbrcld.SharedKernel.Result;
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
    [Route("api/projects")]
    public class ProjectsController : BaseController
    {
        private readonly IMediator mediator;

        public ProjectsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("applicant-projects")]
        public async Task<ActionResult<IList<ListApplicantProjectsViewModel>>> ListApplicantProjects()
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new ListApplicantProjectsQuery(userId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("applicant-completed-projects")]
        public async Task<ActionResult<IList<ListApplicantCompletedProjectsViewModel>>> ListApplicantCompletedProjects()
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new ListApplicantCompletedProjectsQuery(userId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        [Route("underreview-projects/{moduleId}")]
        public async Task<ActionResult<IList<ListInstructorProjectsViewModel>>> ListInstructorProjects([FromRoute] Guid moduleId)
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new ListInstructorProjectsQuery(userId, moduleId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        [Route("instructor-template-project/{moduleId}")]
        public async Task<ActionResult<IList<GetInstructorProjectViewModel>>> GetInstructorProject([FromRoute] Guid moduleId)
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new GetInstructorProjectQuery(moduleId, userId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        [Route("instructor-project")]
        public async Task<ActionResult<Guid>> CreateInstructorProject(CreateInstructorProjectCommand command)
        {
            command.InstructorId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);

            return FromResult(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        [Route("instructor-project")]
        public async Task<ActionResult<Guid>> UpdateInstructorProject(EditInstructorProjectCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        //[Authorize(Roles = "Applicant")]
        //[HttpPut]
        //[Route("applicant-project")]
        //public async Task<ActionResult<Guid>> UpdateApplicantProject(EditApplicantProjectCommand command)
        //{
        //    var result = await mediator.Send(command).ConfigureAwait(false);
        //    return FromResult(result);
        //}
        [Authorize(Roles = "Applicant, Alumni")]
        [HttpPut]
        [Route("applicant-project")]
        public async Task<ActionResult<Guid>> UpdateApplicantProject(IFormFile file, Guid projectId, string name, string name_ar, string description, string description_ar, Guid topicId, int status)
        {
            using var ms = new MemoryStream();
            if (file != null)
            {
                file.CopyTo(ms);
                var command = new EditApplicantProjectCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                userId:User.GetUserId(),
                projectId: projectId,
                name: name,
                name_ar: name_ar,
                description: description,
                description_ar: description_ar,
                topicId: topicId,
                status: status
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
            else
            {
                var command = new EditApplicantProjectCommand(
                content: null,
                contentType: null,
                fileName: null,
                projectId: projectId,
                userId: User.GetUserId(),
                name: name,
                name_ar: name_ar,
                description: description,
                description_ar: description_ar,
                topicId: topicId,
                status: status
                );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
        }

        [Authorize(Roles = "Applicant, Instructor, Alumni")]
        [HttpPost]
        [Route("add-attachment")]
        public async Task<ActionResult> AddAttachment(IFormFile file, Guid projectId, string summary_reason, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            var userId = User.GetUserId();

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new AddAttachmentCommand(
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName,
                projectId: projectId,
                userId: userId,
                summary_reason: summary_reason

                );

                var result = await this.mediator.Send(command, cancellationToken);
                return FromResult(result);
            }
            return FromResult(Result.Failure("No File Attached"));
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        [Route("approve-project")]
        public async Task<ActionResult<Guid>> ApproveProject(SetProjectApprovalCommand command)
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [Authorize(Roles = "Applicant, Alumni")]
        [HttpGet]
        [Route("group-projects")]
        public async Task<ActionResult<IList<ListGroupProjectsViewModel>>> ListGroupProjects()
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new ListGroupProjectsQuery(userId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetProjectByIdViewModel>> GetProjectyId([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var project = await this.mediator.Send(new GetProjectByIdQuery(id));
            if (project.AttachmentUrl != null)
            {
                project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
            }
            return Ok(project);
        }

        [Authorize(Roles = "Alumni")]
        [HttpGet]
        [Route("cohort-projects/{cohortId}")]
        public async Task<ActionResult<IList<ListProjectsByCohortIdViewModel>>> ListCohortProjects([FromRoute] Guid cohortId)
        {
            var userId = User.GetUserId();
            var projects = await this.mediator.Send(new ListProjectsByCohortIdQuery(cohortId, userId));
            foreach (var project in projects)
            {
                if (project.AttachmentUrl != null)
                {
                    project.AttachmentUrl = Url.RouteUrl("GetAttachment", new { Url = project.AttachmentUrl });
                }
            }
            return Ok(projects);
        }
    }
}
