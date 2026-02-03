using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Scholarships.Command
{
    public sealed class AddScholarshipRegistrationCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid Id { get; set; }
        public Guid ScholarshipId { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddScholarshipRegistrationCommand, Result<Guid>>
        {
            private readonly IScholarshipRegistrationRepository scholarshipRegistration;

            public CommandHandler(IScholarshipRegistrationRepository scholarshipRegistration)
            {
                this.scholarshipRegistration = scholarshipRegistration;
            }

            public async Task<Result<Guid>> Handle(AddScholarshipRegistrationCommand request, CancellationToken cancellationToken)
            {
                var scholarshipregistrationRecord = ScholarshipRegistration.Create(request.Id, request.ScholarshipId);
                await scholarshipRegistration.CreateAsync(scholarshipregistrationRecord, cancellationToken).ConfigureAwait(false);
                return Result.Success(scholarshipregistrationRecord.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddScholarshipRegistrationCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ScholarshipId).NotNull().NotEmpty();
                RuleFor(x => x.Id).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
