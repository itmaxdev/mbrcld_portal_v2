using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TeamMember.Commands
{
    public sealed class DeactivateTeamMemberCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<DeactivateTeamMemberCommand, Result>
        {

            private readonly IUniversityTeamMemberRepository teamMemberRepository;

            public CommandHandler(IUniversityTeamMemberRepository teamMemberRepository)
            {
                this.teamMemberRepository = teamMemberRepository;
            }

            public async Task<Result> Handle(DeactivateTeamMemberCommand request, CancellationToken cancellationToken)
            {
                var teamMember = await teamMemberRepository.GetTeamMemberByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (teamMember.HasNoValue)
                {
                    throw new Exception();
                }

                var teamMemberData = teamMember.Value;
                teamMemberData.status = 1; //Inactive

                await teamMemberRepository.UpdateAsync(teamMemberData).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<DeactivateTeamMemberCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
