using Mbrcld.Application.Features.Metadata.Queries;
using Mbrcld.Application.Features.TrainingCourses.Commands;
using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.SharedKernel.Result;
using Mbrcld.Web.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/contents")]
    public class ContentsController : BaseController
    {
        private readonly IMediator mediator;
        public ContentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        [Route("section-contents/{sectionId}")]
        public async Task<ActionResult<IList<ListContentsBySectionIdViewModel>>> GetSectionContents([FromRoute] Guid sectionId)
        {
            var contents = await this.mediator.Send(new ListContentsBySectionIdQuery(sectionId));
            foreach (var content in contents)
            {
                content.DocumentUrl = Url.RouteUrl("GetAttachedFile", new { key = content.Id });
            }
            return Ok(contents);
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpPost]
        public async Task<ActionResult> AddOrEditContent(IFormFile file, [FromForm] Guid sectionId, [FromForm] Guid? contentid, [FromForm] string name, [FromForm] int duration, [FromForm] decimal order, [FromForm] int type, [FromForm] string text, [FromForm] string url, [FromForm] DateTime? startdate)
        {
            using var ms = new MemoryStream();

            Guid content = Guid.Empty;
            if(contentid != null)
            {
                content = contentid.Value;
            }

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new AddOrEditContentCommand(
                 doccontent: ms.ToArray(),
                 contentType: file.ContentType,
                 fileName: file.FileName,
                 contentId: content,
                 sectionId: sectionId,
                 name: name,
                 duration: duration,
                 order: order,
                 type: type, // 1:Text 2:Video 3:Doc 4:Meeting 5:Sticky Note
                 text: text,
                 url: url,
                 startDate: startdate
                 );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
            else
            {
                var command = new AddOrEditContentCommand(
                    doccontent: null,
                    contentType: null,
                    fileName: null,
                    contentId: content,
                    sectionId: sectionId,
                    name: name,
                    duration: duration,
                    order: order,
                    type: type, // 1:Text 2:Video 3:Doc 4:Meeting 5:Sticky Note
                    text: text,
                    url: url,
                    startDate: startdate
                    );

                var result = await this.mediator.Send(command);
                return FromResult(result);
            }
        }

        [Authorize(Roles = "Instructor, Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteContent([FromRoute] Guid id)
        {
            var command = new RemoveContentCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return FromResult(result);
        }
    }
}
