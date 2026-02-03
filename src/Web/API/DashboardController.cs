using Mbrcld.Application.Features.Enrollments.Commands;
using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.SharedKernel.Result;
using Mbrcld.Web.Constants;
using Mbrcld.Web.DTOs;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize(Roles = "Applicant, Direct Manager, Alumni")]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IMediator mediator;

        public DashboardController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardViewModel>> GetDashboardData(Guid? id = null)
        {
            if (id == null)
            {
                id = User.GetUserId();
            }
            var Dashboard = await this.mediator.Send(new DashboardQuery(id.Value));
            return Ok(Dashboard);
        }
    }
}
