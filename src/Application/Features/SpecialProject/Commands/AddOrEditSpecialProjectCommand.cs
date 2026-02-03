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
    public sealed class AddOrEditSpecialProjectCommand : IRequest<Result>
    {
        #region Command
        public Guid SpecialProjectId { get; }
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public int SpecialProjectStatus { get; set; }
        public Guid SpecialProjectTopicId { get; set; }
        public Guid Alumni { get; set; }
        public string Benchmark { get; set; }
        public decimal Budget { get; set; }
        public Guid SectorId { get; set; }
        public string OtherSector { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditSpecialProjectCommand(
            byte[]? content,
            string? contentType,
            string? fileName,
            Guid specialProjectId,
            Guid userId,
            string desrciption,
            string name,
            string body,
            decimal budget,
            string benchmark,
            Guid specialProjectTopicId,
            Guid sectorId,
            string otherSector,
            int specialProjectStatus
            )
        {
            this.SpecialProjectId = specialProjectId;
            this.Alumni = userId;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.Description = desrciption;
            this.Name = name;
            this.Body = body;
            this.SpecialProjectStatus = specialProjectStatus;
            this.SpecialProjectTopicId = specialProjectTopicId;
            this.SectorId = sectorId;
            this.Budget = budget;
            this.Benchmark = benchmark;
            this.OtherSector = otherSector;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditSpecialProjectCommand, Result>
        {
            private readonly IAttachedPictureService attachedPictureService;
            private readonly ISpecialProjectRepository specialProjectRepository;

            public CommandHandler(IAttachedPictureService attachedPictureService, ISpecialProjectRepository specialProjectRepository)
            {
                this.attachedPictureService = attachedPictureService;
                this.specialProjectRepository = specialProjectRepository;
            }

            public async Task<Result> Handle(AddOrEditSpecialProjectCommand request, CancellationToken cancellationToken)
            {
                var specialProjectId = request.SpecialProjectId;
                var specialProject = await specialProjectRepository.GetSpecialProjectByIdAsync(specialProjectId, cancellationToken);
                if (specialProject.HasValue)
                {
                    var specialProjectData = specialProject.Value;
                    specialProjectData.Name = request.Name;
                    specialProjectData.Description = request.Description;
                    specialProjectData.Body = request.Body;
                    specialProjectData.SpecialProjectStatus = request.SpecialProjectStatus;
                    specialProjectData.AlumniId = request.Alumni;
                    specialProjectData.SpecialProjectTopicId = request.SpecialProjectTopicId;
                    specialProjectData.SectorId = request.SectorId;
                    specialProjectData.Budget = request.Budget;
                    specialProjectData.Benchmark = request.Benchmark;
                    specialProjectData.OtherSector = request.OtherSector;

                    await specialProjectRepository.UpdateAsync(specialProjectData).ConfigureAwait(false);
                }
                else
                {
                    var specialProjectData = SpecialProject.Create(
                        description: request.Description,
                        body: request.Body,
                        specialprojectstatus: request.SpecialProjectStatus,
                        userid: request.Alumni,
                        date: DateTime.Now,
                        name: request.Name,
                        budget: request.Budget,
                        specialprojecttopicid: request.SpecialProjectTopicId,
                        sectorid: request.SectorId,
                        benchmark: request.Benchmark,
                        otherSector: request.OtherSector
                    );

                    var returnedSpecialProject = await specialProjectRepository.CreateAsync(specialProjectData).ConfigureAwait(false);
                    specialProjectId = returnedSpecialProject.Value.Id;
                }

                if (request.Content != null)
                {
                    return await this.attachedPictureService.AddOrEditSpecialProjectFileAsync(
                        specialProjectId,
                        request.Content,
                        request.ContentType,
                        request.FileName,
                        cancellationToken
                        );
                }
                else
                {
                    return Result.Success(specialProjectId);
                }
            }
        }
        #endregion
    }
}
