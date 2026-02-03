using Mbrcld.Application.Features.TrainingCourses.Commands;
using Mbrcld.Application.Features.TrainingCourses.Queries;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/profile/training-courses")]
    public class TrainingCoursesController : BaseController
    {
        private readonly IMediator mediator;

        public TrainingCoursesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserTrainingCoursesViewModel>>> GetTrainingCourses()
        {
            var userId = User.GetUserId();
            var trainingCourses = await this.mediator.Send(new ListUserTrainingCoursesQuery(userId));
            return Ok(trainingCourses);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTrainingCourse(AddTrainingCourseCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateTrainingCourse([FromRoute] Guid id, EditTrainingCourseCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteTrainingCourse([FromRoute] Guid id)
        {
            var command = new RemoveTrainingCourseCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
