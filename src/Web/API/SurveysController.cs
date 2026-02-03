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
    [Authorize(Roles = "Applicant, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/suveys")]
    public class SurveysController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IAttachedPictureService eventPictureService;

        public SurveysController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ListUserSurveysUrlViewModel>>> GetUserSurveys()
        {
            var userId = User.GetUserId();
            var surveys = await this.mediator.Send(new ListUserSurveysURLQuery(userId));
            return Ok(surveys);
        }

        [HttpGet]
        [Route("program-surveys")]
        public async Task<ActionResult<IList<ListSurveysByProgramIdViewModel>>> GetProgramSurveys()
        {
            var userId = User.GetUserId();
            var surveys = await this.mediator.Send(new ListSurveysByProgramIdQuery(userId));
            return Ok(surveys);
        }
    }
}
