using Mbrcld.Application.Features.Users.Commands;
using Mbrcld.Application.Features.Users.Queries;
using Mbrcld.Application.Interfaces;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/profile/documents")]
    public class UserDocumentsController : BaseController
    {
        private readonly IAttachedPictureService attachedPictureService;
        private readonly IMediator mediator;

        public UserDocumentsController(IAttachedPictureService attachedPictureService, IMediator mediator)
        {
            this.attachedPictureService = attachedPictureService;
            this.mediator = mediator;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IList<ListUserDocumentsViewModel>>> ListDocuments(CancellationToken cancellationToken)
        {
            var documents = await this.mediator.Send(new ListUserDocumentsQuery(User.GetUserId()), cancellationToken);
            return Ok(documents);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult> AddDocument([FromRoute] string id, IFormFile file, CancellationToken cancellationToken)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);

            var command = new AddOrReplaceUserDocumentCommand(
                userId: User.GetUserId(),
                identitifer: id,
                content: ms.ToArray(),
                contentType: file.ContentType,
                fileName: file.FileName
                );

            var result = await this.mediator.Send(command, cancellationToken);
            return FromResult(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> RemoveDocument([FromRoute] string id)
        {
            var result = await this.mediator.Send(new RemoveUserDocumentCommand(User.GetUserId(), id));
            return FromResult(result);
        }

        [HttpGet]
        [Route("role-user-manual")]
        public async Task<ActionResult> GetRoleUserManualAttachment(CancellationToken cancellationToken)
        {
            string roleName = "";
            var userId = User.GetUserId();
            var profile = await this.mediator.Send(new GetUserProfileQuery(userId));

            if (profile.HasNoValue)
            {
                return NotFound();
            }

            int role = profile.Value.Role;
            switch (role)
            {
                case (int)UserRoles.Registrant:
                    roleName = UserRoles.Registrant.ToString();
                    break;

                case (int)UserRoles.Applicant:
                    roleName = UserRoles.Applicant.ToString();
                    break;

                case (int)UserRoles.Alumni:
                    roleName = UserRoles.Alumni.ToString();
                    break;

                case (int)UserRoles.Instructor:
                    roleName = UserRoles.Instructor.ToString();
                    break;

                case (int)UserRoles.DirectManager:
                    roleName = UserRoles.DirectManager.ToString();
                    break;
            }

            var extension = ".pdf";
            var contentType = "";

            var contents = await attachedPictureService.GetRoleAttachmentAsync(roleName);
            if (contents.HasNoValue)
            {
                return NotFound();
            }

            var filename = Path.GetFileName(roleName) + extension;
            new FileExtensionContentTypeProvider().TryGetContentType(filename, out contentType);

            return File(contents.Value, contentType, filename);
        }
    }
}
