using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Web.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 900, VaryByHeader = "X-Accept-Language")]
    [Route("api/metadata")]
    public class MetadataController : ControllerBase
    {
        private readonly IMediator mediator;

        public MetadataController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("industries")]
        public async Task<ActionResult<IList<ListIndustriesViewModel>>> GetIndustries()
        {            
            var industries = await this.mediator.Send(new ListIndustriesQuery());
            return Ok(industries);
        }

        [HttpGet]
        [Route("sectors")]
        public async Task<ActionResult<IList<ListSectorsViewModel>>> GetSectors()
        {
            var sectors = await this.mediator.Send(new ListSectorsQuery());
            return Ok(sectors);
        }

        [HttpGet]
        [Route("languages")]
        public async Task<ActionResult<IList<ListLanguagesViewModel>>> GetLanguages()
        {
            var languages = await this.mediator.Send(new ListLanguagesQuery());
            return Ok(languages);
        }
    }
}
