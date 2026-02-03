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
    public sealed class AddOrEditMaterialCommand : IRequest<Result>
    {
        #region Command
        public Guid MaterialId { get; }
        public string Name { get; set; }
        public string Name_AR { get; set; }
        public string Location { get; set; }
        public int Duration { get; set; }
        public decimal Order { get; set; }
        public Guid ModuleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public int Status { get; set; }
        public CancellationToken CancellationToken { get; }

        public AddOrEditMaterialCommand(
            Guid materialId,
            Guid moduleId,
            string name,
            string name_ar,
            string location,
            int duration,
            decimal order,
            DateTime? startdate,
            DateTime? publishdate,
            int status
            )
        {
            this.MaterialId = materialId;
            this.ModuleId = moduleId;
            this.Name_AR = name_ar;
            this.Name = name;
            this.Location = location;
            this.Duration = duration;
            this.Order = order;
            this.StartDate = startdate;
            this.PublishDate = publishdate;
            this.Status = status;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<AddOrEditMaterialCommand, Result>
        {
            private readonly IMaterialRepository materialRepository;

            public CommandHandler(IMaterialRepository materialRepository)
            {
                this.materialRepository = materialRepository;
            }

            public async Task<Result> Handle(AddOrEditMaterialCommand request, CancellationToken cancellationToken)
            {
                var materialid = request.MaterialId;
                var material = await materialRepository.GetMaterialByIdAsync(materialid, cancellationToken);
                if (material != null)
                {
                    var materialdata = material;
                    materialdata.Name = request.Name;
                    materialdata.Name_AR = request.Name_AR;
                    materialdata.Location = request.Location;
                    materialdata.Duration = request.Duration;
                    materialdata.ModuleId = request.ModuleId;
                    materialdata.Order = request.Order;
                    materialdata.StartDate = request.StartDate;
                    materialdata.PublishDate = request.PublishDate;
                    materialdata.Status = request.Status;

                    await materialRepository.UpdateAsync(materialdata).ConfigureAwait(false);
                    return Result.Success(materialid);
                }
                else
                {
                    var materialdata = Material.Create(
                        name: request.Name,
                        name_ar: request.Name_AR,
                        location: request.Location,
                        duration: request.Duration,
                        order: request.Order,
                        moduleid: request.ModuleId,
                        startdate: request.StartDate,
                        publishdate: request.PublishDate,
                        status: request.Status
                    );

                    var returnedmaterial = await materialRepository.CreateAsync(materialdata).ConfigureAwait(false);
                    return Result.Success(returnedmaterial.Value.Id);
                }
            }
        }
        #endregion
    }
}
