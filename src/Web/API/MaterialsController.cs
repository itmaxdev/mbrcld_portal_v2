using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.ProgramQuestions.Queries;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Web.Constants;
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
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/materials")]
    public class MaterialsController : BaseController
    {
        private readonly IMediator mediator;
        public MaterialsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        [Route("module-materials/{moduleId}")]
        public async Task<ActionResult<IList<ListMaterialsByModuleIdViewModel>>> GetModuleMaterials([FromRoute] Guid moduleId)
        {
            var userId = User.GetUserId();
            var materials = await this.mediator.Send(new ListMaterialsByModuleIdQuery(moduleId, userId));
            return Ok(materials);
        }

        [Authorize]
        [HttpGet]
        [Route("cohort-materials/{moduleId}")]
        public async Task<ActionResult<IList<ListMaterialsByCohortModuleIdViewModel>>> GetCohortMaterials([FromRoute] Guid moduleId)
        {
            var materials = await this.mediator.Send(new ListMaterialsByCohortModuleIdQuery(moduleId));
            return Ok(materials);
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetMaterialByIdViewModel>> GetMaterial([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var material = await this.mediator.Send(new GetMaterialByIdQuery(id));
            return Ok(material);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditMaterial(Guid materialId, Guid moduleId, string name, string name_ar, string location, int duration, decimal order, DateTime? startdate, DateTime? publishdate, MaterialStatuses status, CancellationToken cancellationToken)
        {

            var command = new AddOrEditMaterialCommand(
                materialId: materialId,
                moduleId: moduleId,
                name: name,
                name_ar: name_ar,
                location: location,
                duration: duration,
                order: order,
                startdate: startdate,
                publishdate: publishdate,
                status: (int)status
                );

            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }
    }
}
