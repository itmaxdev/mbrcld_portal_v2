using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Projects.Commands
{
    public sealed class CreateInstructorProjectCommand : IRequest<Result<Guid>>
    {
        #region Command

        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        public Guid? InstructorId { get; set; }
        public Guid ModuleId { get; set; }

        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<CreateInstructorProjectCommand, Result<Guid>>
        {
            private readonly IProjectRepository ProjectRepository;
            private readonly IModuleRepository ModuleRepository;

            public CommandHandler(IProjectRepository ProjectRepository, IModuleRepository ModuleRepository)
            {
                this.ProjectRepository = ProjectRepository;
                this.ModuleRepository = ModuleRepository;
            }

            public async Task<Result<Guid>> Handle(CreateInstructorProjectCommand request, CancellationToken cancellationToken)
            {
                var module =await ModuleRepository.GetModuleByIdAsync(request.ModuleId);

                var project = Project.Create(
                    name: request.Name,
                    name_ar: request.Name_Ar,
                    description: request.Description,
                    description_ar: request.Description_AR,
                    type: 1, //Accelerated project
                    instructorId: request.InstructorId,
                    applicantId: null,
                    moduleId: request.ModuleId,
                    programId: module.ProgramId,
                    startdate: module.StartDate,
                    enddate: module.EndDate,
                    projectstatus: 1, //Draft
                    template: true
                );

                await ProjectRepository.CreateInstructorProjectAsync(project).ConfigureAwait(false);
                return project.Id;
            }
        }
        #endregion

        #region Command validator
        //public sealed class CommandValidator : AbstractValidator<AddAchievementCommand>
        //{
        //    public CommandValidator()
        //    {
        //        RuleFor(x => x.Description).NotNull().NotEmpty();
        //        RuleFor(x => x.Description_AR).NotNull().NotEmpty();
        //        RuleFor(x => x.SummaryOfAchievement).NotNull().NotEmpty();
        //        RuleFor(x => x.SummaryOfAchievement_AR).NotNull().NotEmpty();
        //        RuleFor(x => x.Organization).NotNull().NotEmpty();
        //        RuleFor(x => x.Organization_AR).NotNull().NotEmpty();
        //        RuleFor(x => x.YearOfAchievement).NotNull().NotEmpty();
        //    }
        //}
        #endregion
    }
}
