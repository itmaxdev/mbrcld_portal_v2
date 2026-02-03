using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Memberships.Commands
{
    public sealed class AddMembershipCommand : IRequest<Result<Guid>>
    {
        #region Command
        public string InstitutionName { get; set; }
        public string InstitutionName_AR { get; set; }
        public DateTime JoinDate { get; set; }
        public string RoleName { get; set; }
        public string RoleName_AR { get; set; }
        public bool? Active { get; set; }
        public Guid ContactId { get; set; }
        public int? MembershipLevel { get; set; }
        public bool Completed { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddMembershipCommand, Result<Guid>>
        {
            private readonly IMembershipRepository membershipRepository;

            public CommandHandler(IMembershipRepository membershipRepository)
            {
                this.membershipRepository = membershipRepository;
            }

            public async Task<Result<Guid>> Handle(AddMembershipCommand request, CancellationToken cancellationToken)
            {
                var membership = Membership.Create(
                    institutionName: request.InstitutionName,
                    institutionName_AR: request.InstitutionName_AR,
                    joinDate: request.JoinDate,
                    roleName: request.RoleName,
                    roleName_AR: request.RoleName_AR,
                    active: request.Active.Value,
                    contactId: request.ContactId,
                    membershipLevel:request.MembershipLevel                    
                    );

                await membershipRepository.CreateAsync(membership).ConfigureAwait(false);

                return membership.Id;
            }
        }
        #endregion

        #region Command validator
        public class CommandValidator : AbstractValidator<AddMembershipCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.InstitutionName).NotNull().NotEmpty();
                RuleFor(x => x.InstitutionName_AR).NotNull().NotEmpty();
                RuleFor(x => x.JoinDate).NotNull().NotEmpty();
                RuleFor(x => x.RoleName).NotNull().NotEmpty();
                RuleFor(x => x.RoleName_AR).NotNull().NotEmpty();
                RuleFor(x => x.Active).NotNull().NotEmpty();
               // RuleFor(x => x.MembershipLevel).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}