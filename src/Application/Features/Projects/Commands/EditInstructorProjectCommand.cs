using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Projects.Commands
{
    public sealed class EditInstructorProjectCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Name_Ar { get; set; }
        public string Description { get; set; }
        public string Description_AR { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditInstructorProjectCommand, Result>
        {
            private readonly IProjectRepository projectRepository;

            public CommandHandler(IProjectRepository projectRepository)
            {
                this.projectRepository = projectRepository;
            }

            public async Task<Result> Handle(EditInstructorProjectCommand request, CancellationToken cancellationToken)
            {
                var project = await projectRepository.GetProjectByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (project.HasNoValue)
                {
                    throw new Exception();
                }

                var projectData = project.Value;
                projectData.Description = request.Description;
                projectData.Description_Ar = request.Description_AR;
                projectData.Name = request.Name;
                projectData.Name_Ar = request.Name_Ar;

                await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditInstructorProjectCommand>
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
