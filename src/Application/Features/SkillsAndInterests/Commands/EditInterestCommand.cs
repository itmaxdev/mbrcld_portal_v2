using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.SkillsAndInterests.Commands
{
    public sealed class EditInterestCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Name { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditInterestCommand, Result>
        {
            private readonly IInterestRepository interestRepository;

            public CommandHandler(IInterestRepository interestRepository)
            {
                this.interestRepository = interestRepository;
            }

            public async Task<Result> Handle(EditInterestCommand request, CancellationToken cancellationToken)
            {
                var interestPull = await interestRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (interestPull.HasNoValue)
                {
                    throw new Exception();
                }

                var interset = interestPull.Value;
                interset.Name = request.Name;

                await interestRepository.UpdateAsync(interset).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditInterestCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEqual(default(Guid));
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
