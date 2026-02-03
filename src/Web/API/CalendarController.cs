using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Applicant, Direct Manager, Alumni, Registrant")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator mediator;

        public CalendarController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<GetUserCalendarViewModel>>> GetUserCalendar(Guid? id = null)
        {
            if (id == null)
            {
                id = User.GetUserId();
            }
            var calendar = await this.mediator.Send(new GetUserCalendarQuery(id.Value));
            return Ok(calendar);
        }

        [HttpGet]
        [Route("meetings")]
        public async Task<ActionResult<IList<ListApplicantMeetingViewModel>>> ListMeeting(Guid? id = null)
        {
            if (id == null)
            {
                id = User.GetUserId();
            }
            var calendar = await this.mediator.Send(new ListApplicantMeetingQuery(id.Value));
            return Ok(calendar);
        }
    }
}
