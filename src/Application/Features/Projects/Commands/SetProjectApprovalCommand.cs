using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Projects.Commands
{
    public sealed class SetProjectApprovalCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string ApprovalReason { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<SetProjectApprovalCommand, Result>
        {
            private readonly IProjectRepository projectRepository;

            public CommandHandler(IProjectRepository projectRepository)
            {
                this.projectRepository = projectRepository;
            }

            public async Task<Result> Handle(SetProjectApprovalCommand request, CancellationToken cancellationToken)
            {
                var project = await projectRepository.GetProjectByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (project.HasNoValue)
                {
                    throw new Exception();
                }

                var projectData = project.Value;
                projectData.AdminApproval = 1; //Approved
                projectData.ApprovalReason = request.ApprovalReason;

                await projectRepository.UpdateAsync(projectData).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<SetProjectApprovalCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
