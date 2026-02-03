using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.References.Commands
{
    public sealed class AddReferenceCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
        public string FullName_AR { get; set; }
        public string JobTitle { get; set; }
        public string JobTitle_AR { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationName_AR { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddReferenceCommand, Result<Guid>>
        {
            private readonly IReferenceRepository reference;

            public CommandHandler(IReferenceRepository reference)
            {
                this.reference = reference;
            }

            public async Task<Result<Guid>> Handle(AddReferenceCommand request, CancellationToken cancellationToken)
            {
                var referenceRecord = Reference.Create(
                    request.ContactId,
                    request.FullName,
                    request.FullName_AR,
                    request.JobTitle,
                    request.JobTitle_AR,
                    request.OrganizationName,
                    request.OrganizationName_AR,
                    request.Email,
                    request.Mobile
                    );

                await reference.CreateAsync(referenceRecord, cancellationToken).ConfigureAwait(false);

                return Result.Success(referenceRecord.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddReferenceCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FullName).NotNull().NotEmpty();
                RuleFor(x => x.FullName_AR).NotNull().NotEmpty();
                RuleFor(x => x.JobTitle).NotNull().NotEmpty();
                RuleFor(x => x.JobTitle_AR).NotNull().NotEmpty();
                RuleFor(x => x.OrganizationName).NotNull().NotEmpty();
                RuleFor(x => x.OrganizationName_AR).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Mobile).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
