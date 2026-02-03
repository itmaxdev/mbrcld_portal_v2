using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class AddOrEditProjectIdeaCommand : IRequest<Result>
    {
        #region Command
        public Guid ProjectIdeaId { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int ProjectIdeaStatus { get; set; }
        public decimal Budget { get; set; }
        public Guid Alumni { get; set; }
        public string Benchmark { get; set; }
        public Guid SectorId { get; set; }
        public string OtherSector { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditProjectIdeaCommand(
            byte[]? content,
            string? contentType,
            string? fileName,
            Guid projectIdeaId,
            Guid userId,
            string desrciption,
            string name,
            string body,
            decimal budget,
            int projectIdeaStatus,
            Guid sectorId,
            string benchmark,
            string otherSector
            )
        {
            this.ProjectIdeaId = projectIdeaId;
            this.Alumni = userId;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.Description = desrciption;
            this.Name = name;
            this.Body = body;
            this.ProjectIdeaStatus = projectIdeaStatus;
            this.Budget = budget;
            this.SectorId = sectorId;
            this.Benchmark = benchmark;
            this.OtherSector = otherSector;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditProjectIdeaCommand, Result>
        {
            private readonly IAttachedPictureService attachedPictureService;
            private readonly IProjectIdeaRepository projectIdeaRepository;

            public CommandHandler(IAttachedPictureService attachedPictureService, IProjectIdeaRepository projectIdeaRepository)
            {
                this.attachedPictureService = attachedPictureService;
                this.projectIdeaRepository = projectIdeaRepository;
            }

            public async Task<Result> Handle(AddOrEditProjectIdeaCommand request, CancellationToken cancellationToken)
            {
                var projectIdeaId = request.ProjectIdeaId;
                var projectIdea = await projectIdeaRepository.GetProjectIdeaByIdAsync(projectIdeaId, cancellationToken);
                if (projectIdea.HasValue)
                {
                    var projectIdeadata = projectIdea.Value;
                    projectIdeadata.Name = request.Name;
                    projectIdeadata.Description = request.Description;
                    projectIdeadata.Body = request.Body;
                    projectIdeadata.ProjectIdeaStatus = request.ProjectIdeaStatus;
                    projectIdeadata.AlumniId = request.Alumni;
                    projectIdeadata.Budget = request.Budget;
                    projectIdeadata.SectorId = request.SectorId;
                    projectIdeadata.Benchmark = request.Benchmark;
                    projectIdeadata.OtherSector = request.OtherSector;

                    await projectIdeaRepository.UpdateAsync(projectIdeadata).ConfigureAwait(false);
                }
                else
                {
                    var projectIdeadata = ProjectIdea.Create(
                        description: request.Description,
                        body: request.Body,
                        projectideastatus: request.ProjectIdeaStatus,
                        userid: request.Alumni,
                        date: DateTime.Now,
                        name: request.Name,
                        budget: request.Budget,
                        sectorid: request.SectorId,
                        benchmark: request.Benchmark,
                        otherSector: request.OtherSector
                    );

                    var returnedProjectIdea = await projectIdeaRepository.CreateAsync(projectIdeadata).ConfigureAwait(false);
                    projectIdeaId = returnedProjectIdea.Value.Id;
                }

                if (request.Content != null)
                {
                    return await this.attachedPictureService.AddOrEditProjectIdeaFileAsync(
                        projectIdeaId,
                        request.Content,
                        request.ContentType,
                        request.FileName,
                        cancellationToken
                        );
                }
                else
                {
                    return Result.Success(projectIdeaId);
                }
            }
        }
        #endregion
    }
}
