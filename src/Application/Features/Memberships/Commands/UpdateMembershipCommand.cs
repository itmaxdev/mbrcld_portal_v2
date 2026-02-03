using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Memberships.Commands
{
    public sealed class EditMembershipCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionName_AR { get; set; }
        public DateTime JoinDate { get; set; }
        public string RoleName { get; set; }
        public string RoleName_AR { get; set; }
        public bool? Active { get; set; }
        public int? MembershipLevel { get; set; }
        public bool Completed { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditMembershipCommand, Result>
        {
            private readonly IMembershipRepository membershipRepository;

            public CommandHandler(IMembershipRepository membershipRepository)
            {
                this.membershipRepository = membershipRepository;
            }

            public async Task<Result> Handle(EditMembershipCommand request, CancellationToken cancellationToken)
            {
                var membershipPull = await membershipRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (membershipPull.HasNoValue)
                {
                    throw new Exception();
                }

                var membershipValue = membershipPull.Value;
                membershipValue.InstitutionName = request.InstitutionName;
                membershipValue.InstitutionName_AR = request.InstitutionName_AR;
                membershipValue.JoinDate = request.JoinDate;
                membershipValue.RoleName = request.RoleName;
                membershipValue.RoleName_AR = request.RoleName_AR;
                membershipValue.Active = request.Active.Value;
                membershipValue.MembershipLevel = request.MembershipLevel;
                await membershipRepository.UpdateAsync(membershipValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public class CommandValidator : AbstractValidator<EditMembershipCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.InstitutionName).NotEmpty();
                RuleFor(x => x.InstitutionName_AR).NotEmpty();
                RuleFor(x => x.JoinDate).NotEmpty();
                RuleFor(x => x.RoleName).NotEmpty();
                RuleFor(x => x.RoleName_AR).NotEmpty();
                RuleFor(x => x.Active).NotEmpty();
              //  RuleFor(x => x.MembershipLevel).NotEmpty();
            }
        }
        #endregion
    }
}
