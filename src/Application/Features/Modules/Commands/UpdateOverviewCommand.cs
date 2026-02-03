using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Modules.Commands
{
    public sealed class UpdateOverviewCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Overview { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<UpdateOverviewCommand, Result>
        {
            private readonly IModuleRepository moduleRepository;

            public CommandHandler(IModuleRepository moduleRepository)
            {
                this.moduleRepository = moduleRepository;
            }

            public async Task<Result> Handle(UpdateOverviewCommand request, CancellationToken cancellationToken)
            {
                var module = await moduleRepository.GetModuleByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (module == null)
                {
                    throw new Exception();
                }

                var moduleValue = module;
                moduleValue.Overview = request.Overview;
                moduleValue.EliteClubId = new Guid("809592b9-65b9-ec11-b819-00155d299759");
                await moduleRepository.UpdateAsync(moduleValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<UpdateOverviewCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
                RuleFor(x => x.Overview).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
