using Mbrcld.Application.Features.References.Commands;
using Mbrcld.Application.Features.References.Queries;
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
    [Route("api/profile/references")]
    public class ReferencesController : BaseController
    {
        private readonly IMediator mediator;

        public ReferencesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserReferencesViewModel>>> ListReferences()
        {
            var userId = User.GetUserId();
            var references = await mediator.Send(new ListUserReferencesQuery(userId)).ConfigureAwait(false);
            return Ok(references);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddReference(AddReferenceCommand command)
        {
            command.ContactId = User.GetUserId();
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> EditReference([FromRoute] Guid id, EditReferenceCommand command)
        {
            command.Id = id;
            await mediator.Send(command).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveReference([FromRoute] Guid id)
        {
            await mediator.Send(new RemoveReferenceCommand(id)).ConfigureAwait(false);
            return Ok();
        }
    }
}
