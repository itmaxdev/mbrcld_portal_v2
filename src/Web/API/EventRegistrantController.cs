using Mbrcld.Application.Features.Enrollments.Commands;
using Mbrcld.Application.Features.Events.Queries;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.ProfessionalExperiences.Queries;
using Mbrcld.SharedKernel.Result;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Applicant , Registrant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/eventregistrant")]
    public class EventRegistrantController : BaseController
    {
        private readonly IMediator mediator;

        public EventRegistrantController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserEventRegistrantViewModel>>> GetEventRegistrant()
        {
            var userId = User.GetUserId();
            var eventRegistrant = await mediator.Send(new ListUserEventRegistrantQuery(userId)).ConfigureAwait(false);
            //if (eventRegistrant == null || !eventRegistrant.Any())
            //{
            //    return NotFound("No event registrants found for the user.");
            //}

            return Ok(eventRegistrant);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddEventRegistrant(Guid eventId)
        {
            var command = new AddEventRegistrantCommand
            {
                EventId = eventId,
                Id = User.GetUserId()
            };

            var result = await this.mediator.Send(command);
            return FromResult(result);
        }

        [HttpPost]
        [Route("withanswers")]
        public async Task<ActionResult<Guid>> SaveEventWithAnswers(Guid eventId, [FromBody] EventAnswersForUpsertDto[] dtos)
        {
            try
            {
                var event_command = new AddEventRegistrantCommand
                {
                    EventId = eventId,
                    Id = User.GetUserId()
                };

                var event_result = await this.mediator.Send(event_command);

                if (event_result.IsSuccess)
                {
                    var results = new ArrayList();
                    foreach (var dto in dtos)
                    {
                        var command = new AddEventAnswersCommand
                        {
                            Answer = dto.Answer,
                            EventId = eventId,
                            EventQuestionId = dto.EventQuestionId,
                            EventRegistrantId = event_result.Value
                        };
                        var result = await this.mediator.Send(command);
                        results.Add(result);
                    }
                    return Ok(results);
                }
                else
                {
                    return Ok(event_result);
                }

            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }
    }
    
}
