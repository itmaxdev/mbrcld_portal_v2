using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.LanguageSkills.Commands
{
    public class RemoveLanguageSkillCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; }

        public RemoveLanguageSkillCommand(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RemoveLanguageSkillCommand, Result>
        {
            private readonly ILanguageSkillRepository languageSkillRepository;

            public CommandHandler(ILanguageSkillRepository languageSkillRepository)
            {
                this.languageSkillRepository = languageSkillRepository;
            }

            public async Task<Result> Handle(RemoveLanguageSkillCommand request, CancellationToken cancellationToken)
            {
                await languageSkillRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
                return Result.Success();
            }
        }
        #endregion
    }
}
