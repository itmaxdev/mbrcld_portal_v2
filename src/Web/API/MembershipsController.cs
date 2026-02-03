using AutoMapper.Internal;
using Mbrcld.Application.Features.Memberships.Commands;
using Mbrcld.Application.Features.Memberships.Queries;
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
    [Route("api/profile/memberships")]
    public class MembershipsController : BaseController
    {
        private readonly IMediator mediator;

        public MembershipsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserMembershipsViewModel>>> GetMemberships()
        {
            var userId = User.GetUserId();
            var memberships = await this.mediator.Send(new ListUserMembershipsQuery(userId));
            memberships.ForAll(item => {
           
                    item.Completed = !string.IsNullOrEmpty(item.InstitutionName) &&
                    !string.IsNullOrEmpty(item.InstitutionName_AR) &&
                    item.JoinDate != default(DateTime) &&
                    !string.IsNullOrEmpty(item.RoleName) &&
                    !string.IsNullOrEmpty(item.RoleName_AR) &&
                    item.MembershipLevel != null;
            });
            return Ok(memberships);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddMembership(AddMembershipCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.InstitutionName) &&
                               !string.IsNullOrEmpty(command.InstitutionName_AR) &&
                               command.JoinDate != default(DateTime) &&
                               !string.IsNullOrEmpty(command.RoleName) &&
                               !string.IsNullOrEmpty(command.RoleName_AR) &&
                               command.MembershipLevel != null;
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditMembership([FromRoute] Guid id, EditMembershipCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command).ConfigureAwait(false);
            command.Completed = !string.IsNullOrEmpty(command.InstitutionName) &&
                                           !string.IsNullOrEmpty(command.InstitutionName_AR) &&
                                           command.JoinDate != default(DateTime) &&
                                           !string.IsNullOrEmpty(command.RoleName) &&
                                           !string.IsNullOrEmpty(command.RoleName_AR) &&
                                           command.MembershipLevel != null;
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteMembership([FromRoute] Guid id)
        {
            var result = await mediator.Send(new DeleteMembershipCommand(id)).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
