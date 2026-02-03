using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Universities.Commands
{
    public sealed class UpdateMembershipAttendanceStatusCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public int AttendanceStatus { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<UpdateMembershipAttendanceStatusCommand, Result>
        {
            private readonly IEliteMembershipAttendanceRepository eliteMembershipAttendanceRepository;
            public CommandHandler(IEliteMembershipAttendanceRepository eliteMembershipAttendanceRepository)
            {
                this.eliteMembershipAttendanceRepository = eliteMembershipAttendanceRepository;
            }

            public async Task<Result> Handle(UpdateMembershipAttendanceStatusCommand request, CancellationToken cancellationToken)
            {
                var eliteMembershipAttendance = await eliteMembershipAttendanceRepository.GetEliteMembershipAttendanceByIdAsync(request.Id, cancellationToken);
                if (eliteMembershipAttendance.HasNoValue)
                {
                    return Result.Failure($"Invalid University with ID { request.Id}");
                }

                var eliteMembershipAttendanceValue = eliteMembershipAttendance.Value;
                eliteMembershipAttendanceValue.AttendanceStatus = request.AttendanceStatus;

                await eliteMembershipAttendanceRepository.UpdateAsync(eliteMembershipAttendanceValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<UpdateMembershipAttendanceStatusCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
                RuleFor(x => x.AttendanceStatus).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
