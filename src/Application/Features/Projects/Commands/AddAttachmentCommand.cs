using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Projects.Commands
{
    public sealed class AddAttachmentCommand : IRequest<Result>
    {
        #region Command
        public Guid ProjectId { get; }
        public Guid UserId { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string Summary_Reason { get; }

        public CancellationToken CancellationToken { get; }

        public AddAttachmentCommand(
            byte[]? content,
            string? contentType,
            string? fileName,
            Guid projectId,
            Guid userId,
            string summary_reason
            )
        {
            this.ProjectId = projectId;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.UserId = userId;
            this.Summary_Reason = summary_reason;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddAttachmentCommand, Result>
        {
            private readonly IAttachedPictureService attachedPictureService;
            private readonly IProjectRepository projectRepository;
            private readonly IUserRepository userRepository;


            public CommandHandler(IAttachedPictureService attachedPictureService, IProjectRepository projectRepository, IUserRepository userRepository)
            {
                this.attachedPictureService = attachedPictureService;
                this.projectRepository = projectRepository;
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(AddAttachmentCommand request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
                var project = await projectRepository.GetProjectByIdAsync(request.ProjectId, cancellationToken).ConfigureAwait(false);

                if (project.HasNoValue)
                {
                    throw new Exception();
                }

                var extension = Path.GetExtension(request.FileName);
                var upload = await this.attachedPictureService.AddAttachmentAsync(
                        request.ProjectId,
                        request.Content,
                        request.ContentType,
                        request.FileName,
                        cancellationToken
                        );

                if (upload.IsSuccess)
                {
                    if (user.Value.Role == 2) // Applicant
                    {
                        var projectData = project.Value;
                        projectData.ProjectStatus = 4; // Under Review
                        projectData.Summary = request.Summary_Reason;
                        projectData.ProjectAttachmentUrl = "\\" + request.ProjectId + "\\" + request.ProjectId + extension;
                        await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);
                        return Result.Success();
                    }
                    if (user.Value.Role == 4) // Instructor
                    {
                        var projectData = project.Value;
                        projectData.AdminApproval = 2; // Rejected
                        projectData.Reason = request.Summary_Reason;
                        projectData.ProjectAttachmentUrl = "\\" + request.ProjectId + "\\" + request.ProjectId + extension;
                        await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);
                        return Result.Success();
                    }
                }
                return Result.Failure("Failed while Uploading File");
            }
        }
        #endregion
    }
}
