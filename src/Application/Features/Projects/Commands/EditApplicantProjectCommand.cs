using FluentValidation;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Projects.Commands
{
    public sealed class EditApplicantProjectCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public Guid TopicId { get; set; }
        public int Status { get; set; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }

        public EditApplicantProjectCommand(
            byte[]? content,
            string? contentType,
            string? fileName,
            Guid projectId,
            Guid userId,
            Guid topicId,
            string name,
            string name_ar,
            string description,
            string description_ar,
            int status
            )
        {
            this.Id = projectId;
            this.UserId = userId;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.TopicId = topicId;
            this.Name = name;
            this.Name_Ar = name_ar;
            this.Description = description;
            this.Description_AR = description_ar;
            this.Status = status;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditApplicantProjectCommand, Result>
        {
            private readonly IProjectRepository projectRepository;
            private readonly ITopicRepository topicRepository;
            private readonly IAttachedPictureService attachedPictureService;

            public CommandHandler(IProjectRepository projectRepository, ITopicRepository topicRepository, IAttachedPictureService attachedPictureService)
            {
                this.projectRepository = projectRepository;
                this.attachedPictureService = attachedPictureService;
                this.topicRepository = topicRepository;
            }

            public async Task<Result> Handle(EditApplicantProjectCommand request, CancellationToken cancellationToken)
            {

                var project = await projectRepository.GetProjectByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

                if (project.HasNoValue)
                {
                    throw new Exception();
                }

                if (project.Value.Type == 3) //Applied Research Project
                {
                    var topic = await topicRepository.GetTopicByIdAsync(request.TopicId);
                    if (topic.HasValue)
                    {
                        var topicproject = await projectRepository.ListProjectsByTopicIdAsync(request.TopicId, request.UserId, cancellationToken).ConfigureAwait(false);
                        int topicmaxcount = topic.Value.MaxCount;
                        if (topicproject.Count < topicmaxcount)
                        {
                            if (request.Content != null)
                            {
                                var extension = Path.GetExtension(request.FileName);
                                var upload = await this.attachedPictureService.AddAttachmentAsync(
                                        request.Id,
                                        request.Content,
                                        request.ContentType,
                                        request.FileName,
                                        cancellationToken
                                        );

                                if (upload.IsSuccess)
                                {
                                    var projectData = project.Value;
                                    projectData.Description = request.Description;
                                    projectData.Description_Ar = request.Description_AR;
                                    projectData.Name = request.Name;
                                    projectData.Name_Ar = request.Name_Ar;
                                    projectData.ProjectStatus = request.Status;
                                    projectData.TopicId = request.TopicId;
                                    projectData.ProjectAttachmentUrl = "\\" + request.Id + "\\" + request.Id + extension;

                                    await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                                    return Result.Success();
                                }
                                else
                                {
                                    return Result.Failure("Failed while Uploading File");
                                }
                            }
                            else
                            {
                                var projectData = project.Value;
                                projectData.Description = request.Description;
                                projectData.Description_Ar = request.Description_AR;
                                projectData.Name = request.Name;
                                projectData.Name_Ar = request.Name_Ar;
                                projectData.ProjectStatus = request.Status;
                                projectData.TopicId = request.TopicId;

                                await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                                return Result.Success();
                            }
                        }
                        else
                        {
                            return Result.Failure("Topic reached the Maximum capacity");
                        }
                    }
                    else
                    {
                        return Result.Failure("Topic not Found");
                    }
                }
                else
                {
                    if (request.Content != null)
                    {
                        var extension = Path.GetExtension(request.FileName);
                        var upload = await this.attachedPictureService.AddAttachmentAsync(
                                request.Id,
                                request.Content,
                                request.ContentType,
                                request.FileName,
                                cancellationToken
                                );

                        if (upload.IsSuccess)
                        {
                            var projectData = project.Value;
                            projectData.Description = request.Description;
                            projectData.Description_Ar = request.Description_AR;
                            projectData.Name = request.Name;
                            projectData.Name_Ar = request.Name_Ar;
                            projectData.ProjectStatus = request.Status;
                            projectData.TopicId = null;
                            projectData.ProjectAttachmentUrl = "\\" + request.Id + "\\" + request.Id + extension;

                            await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                            return Result.Success();
                        }
                        else
                        {
                            return Result.Failure("Failed while Uploading File");
                        }
                    }
                    else
                    {
                        var projectData = project.Value;
                        projectData.Description = request.Description;
                        projectData.Description_Ar = request.Description_AR;
                        projectData.Name = request.Name;
                        projectData.Name_Ar = request.Name_Ar;
                        projectData.ProjectStatus = request.Status;
                        projectData.TopicId = null;

                        await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                        return Result.Success();
                    }
                }
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditApplicantProjectCommand>
        {
            public CommandValidator()
            {
                ////RuleFor(x => x.Id).NotNull().NotEmpty();
                RuleFor(x => x.Id).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
