using Mbrcld.Application.Features.Enrollments.Commands;
using Mbrcld.Application.Features.Enrollments.Queries;
using Mbrcld.Application.Features.ProgramAnswers.Commands;
using Mbrcld.Application.Features.ProgramAnswers.Queries;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Registrant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/enrollments")]
    public class EnrollmentsController : BaseController
    {
        private readonly IMediator mediator;

        public EnrollmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetAllEnrollments")]
        public async Task<ActionResult<IList<ListEnrollmentByUserIdViewModel>>> GetAllEnrollments()
        {
            var userId = User.GetUserId();
            var enrollments = await this.mediator.Send(new ListEnrollmentByUserIdQuery(userId));
            return Ok(enrollments);
        }

        [HttpGet("{id}", Name = "GetEnrollmentById")]
        public async Task<ActionResult<GetEnrollmentByIdViewModel>> GetEnrollmentById([FromRoute] Guid id)
        {
            var enrollment = await this.mediator.Send(new GetEnrollmentByIdQuery(id));
            return Ok(enrollment);
        }

        [HttpPost(Name = "AddEnrollment")]
        public async Task<ActionResult<Guid>> AddEnrollment(AddEnrollmentCommand command)
        {
            var userId = User.GetUserId();
            command.Id = userId;
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpPut("{id}/complete", Name = "CompleteEnrollment")]
        public async Task<ActionResult> CompleteEnrollment([FromRoute] Guid id)
        {
            var result = await this.mediator.Send(new CompleteEnrollmentCommand(id));
            return FromResult(result);
        }

        [HttpGet("{id}/answers", Name = "GetAllEnrollmentAnswers")]
        public async Task<ActionResult<IList<ListProgramAnswersByEnrollmentIdViewModel>>> GetAnswers([FromRoute] Guid id)
        {
            var programresults = await this.mediator.Send(new ListProgramAnswersByEnrollmentIdQuery(id));
            return Ok(programresults);
        }

        [HttpPatch("{id}/answers", Name = "UpdateEnrollmentAnswers")]
        public async Task<ActionResult> UpdateEnrollmentAnswers([FromRoute] Guid id, AnswerForUpsertDto[] dtos)
        {
            var command = new AddEditProgramAnswerCommand(id, dtos.ToDictionary(x => x.QuestionId, x => x.AnswerText));
            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        
        [HttpGet("{id}/enrollmentStatus", Name = "GetEnrollmentStatus")]
        public async Task<ActionResult<GetEnrollmentStatusByIdViewModel>> GetEnrollmentStatus([FromRoute] Guid id)
        {
            var result = await this.mediator.Send(new GetEnrollmentStatusByIdQuery(id));
            return Ok(result);
        }
    }
}
