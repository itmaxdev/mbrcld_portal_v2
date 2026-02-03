using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.SkillsAndInterests.Commands
{
    public sealed class AddInterestCommand : IRequest<Result<Guid>>
    {
        #region CommandMembership
        public string Name { get; set; }
        public Guid ContactId { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddInterestCommand, Result<Guid>>
        {
            private readonly IInterestRepository interestRepository;

            public CommandHandler(IInterestRepository interestRepository)
            {
                this.interestRepository = interestRepository;
            }

            public async Task<Result<Guid>> Handle(AddInterestCommand request, CancellationToken cancellationToken)
            {
                var interest = Interest.Create(
                    name: request.Name,
                    contactId: request.ContactId
                    );

                await interestRepository.CreateAsync(interest).ConfigureAwait(false);

                return interest.Id;
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddInterestCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
