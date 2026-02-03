using AutoMapper.Internal;
using Mbrcld.Application.Features.Achievements.Commands;
using Mbrcld.Application.Features.Achievements.Queries;
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
    [Route("api/profile/achievements")]
    public class AchievementsController : BaseController
    {
        private readonly IMediator mediator;

        public AchievementsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserAchievementsViewModel>>> GetAchievements()
        {
            var userId = User.GetUserId();
            var achievements = await this.mediator.Send(new ListUserAchievementsQuery(userId));
            achievements.ForAll(item => {
                item.Completed = !string.IsNullOrEmpty(item.Description) &&
                                 !string.IsNullOrEmpty(item.Description_AR) &&
                                 !string.IsNullOrEmpty(item.SummaryOfAchievement) &&
                                 !string.IsNullOrEmpty(item.SummaryOfAchievement_AR) &&
                                 !string.IsNullOrEmpty(item.Organization) &&
                                 !string.IsNullOrEmpty(item.Organization_AR) &&
                                 !string.IsNullOrEmpty(item.YearOfAchievement) &&
                                 item.PopulationImpact !=null &&
                                 item.FinancialImpact != null &&
                                 item.AchievementImpact != null;          
                   });

            return Ok(achievements);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddAchievement(AddAchievementCommand command)
        {
            command.ContactId = User.GetUserId();

            var result = await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.Description) &&
                                            !string.IsNullOrEmpty(command.Description_AR) &&
                                            !string.IsNullOrEmpty(command.SummaryOfAchievement) &&
                                            !string.IsNullOrEmpty(command.SummaryOfAchievement_AR) &&
                                            !string.IsNullOrEmpty(command.Organization) &&
                                            !string.IsNullOrEmpty(command.Organization_AR) &&
                                            !string.IsNullOrEmpty(command.YearOfAchievement) &&
                                            command.PopulationImpact != null &&
                                            command.FinancialImpact != null &&
                                            command.AchievementImpact != null;
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditAchievement([FromRoute] Guid id, EditAchievementCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.Description) &&
                                            !string.IsNullOrEmpty(command.Description_AR) &&
                                            !string.IsNullOrEmpty(command.SummaryOfAchievement) &&
                                            !string.IsNullOrEmpty(command.SummaryOfAchievement_AR) &&
                                            !string.IsNullOrEmpty(command.Organization) &&
                                            !string.IsNullOrEmpty(command.Organization_AR) &&
                                            !string.IsNullOrEmpty(command.YearOfAchievement) &&
                                            command.PopulationImpact != null &&
                                            command.FinancialImpact != null &&
                                            command.AchievementImpact != null;
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveAchievement([FromRoute] Guid id)
        {
            var result = await mediator.Send(new RemoveAchievementCommand(id)).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
