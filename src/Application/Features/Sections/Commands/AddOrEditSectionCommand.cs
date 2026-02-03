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
    public sealed class AddOrEditSectionCommand : IRequest<Result>
    {
        #region Command
        public Guid SectionId { get; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public Guid MaterialId { get; set; }
        public Guid ContactId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int SectionStatus { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditSectionCommand(
            Guid materialId,
            Guid sectionId,
            Guid userId,
            string name,
            string name_ar,
            int duration,
            decimal order,
            DateTime? startdate,
            DateTime? publishdate,
            int sectionstatus
            )
        {
            this.MaterialId = materialId;
            this.SectionId = sectionId;
            this.ContactId = userId;
            this.Name_AR = name_ar;
            this.Name = name;
            this.Duration = duration;
            this.Order = order;
            this.StartDate = startdate;
            this.PublishDate = publishdate;
            this.SectionStatus = sectionstatus;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditSectionCommand, Result>
        {
            private readonly ISectionRepository sectionRepository;

            public CommandHandler(ISectionRepository sectionRepository)
            {
                this.sectionRepository = sectionRepository;
            }

            public async Task<Result> Handle(AddOrEditSectionCommand request, CancellationToken cancellationToken)
            {
                var sectionid = request.SectionId;
                var section = await sectionRepository.GetSectionByIdAsync(sectionid, request.ContactId, cancellationToken);
                if (section != null)
                {
                    var sectiondata = section;
                    sectiondata.Name = request.Name;
                    sectiondata.Name_AR = request.Name_AR;
                    sectiondata.Duration = request.Duration;
                    sectiondata.MaterialId = request.MaterialId;
                    sectiondata.Order = request.Order;
                    sectiondata.StartDate = request.StartDate;
                    sectiondata.PublishDate = request.PublishDate;
                    sectiondata.SectionStatus = request.SectionStatus;

                    await sectionRepository.UpdateAsync(sectiondata).ConfigureAwait(false);
                    return Result.Success(sectionid);
                }
                else
                {
                    var sectiondata = Section.Create(
                        name: request.Name,
                        name_ar: request.Name_AR,
                        duration: request.Duration,
                        order: request.Order,
                        materialid: request.MaterialId,
                        startdate: request.StartDate,
                        publishdate: request.PublishDate,
                        sectionstatus: request.SectionStatus
                    );

                    var returnedsection = await sectionRepository.CreateAsync(sectiondata).ConfigureAwait(false);
                    return Result.Success(returnedsection.Value.Id);
                }
            }
        }
        #endregion
    }
}
