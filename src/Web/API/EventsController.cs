using Mbrcld.Application.Features.Enrollments.Commands;
using Mbrcld.Application.Features.EventQuestions.Queries;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.ProgramQuestions.Queries;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Applicant , Registrant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IAttachedPictureService eventPictureService;

        public EventsController(IMediator mediator, IAttachedPictureService eventPictureService)
        {
            this.mediator = mediator;
            this.eventPictureService = eventPictureService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ListEventsViewModel>>> GetEvents()
        {
            var userId = User.GetUserId();

            var events = await this.mediator.Send(new ListEventsQuery(userId));
            foreach (var eventrec in events)
            {
                eventrec.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = eventrec.Id });
            }
            return Ok(events);
        }

        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiProblemDetails), 422)]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetEventById([FromRoute] Guid id,  CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var events = await this.mediator.Send(new GetEventByIdQuery(id, userId));
            if(events != null)
            {
                events.PictureUrl = Url.RouteUrl("GetAttachedPicture", new { key = id });
            }
            return Ok(events);
        }

        [HttpGet]
        [Route("{id}/questions")]
        public async Task<ActionResult<IList<ListEventQuestionByEventIdViewModel>>> GetEventQuestions([FromRoute] Guid id)
        {
            var eventresults = await this.mediator.Send(new ListEventQuestionByEventIdQuery(id));
            return Ok(eventresults);
        }

     /*   [HttpPost]
        [Route("answers")]
        public async Task<ActionResult<Guid>> GetAnswers([FromBody] EventAnswersForUpsertDto[] dtos)
        {
            try
            {
                var results = new ArrayList();
                foreach (var dto in dtos)
                {
                    var command = new AddEventAnswersCommand
                    {
                        Answer = dto.Answer,
                        EventId = dto.EventId,
                        EventQuestionId = dto.EventQuestionId,
                        EventRegistrantId = dto.EventRegistrantId
                    };
                    var result = await this.mediator.Send(command);
                    results.Add(result);
                    await this.mediator.Send(command);
                }
                return Ok(results);
            }
            catch (Exception e)
            {
                return Ok(e);
            }

            //var result = await this.mediator.Send(AddEventAnswersCommand());
            //return result;

            /*var programresults = await this.mediator.Send(new string());
            return Ok(programresults);
        } */
    }
}
