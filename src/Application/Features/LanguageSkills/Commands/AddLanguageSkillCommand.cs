using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.LanguageSkills.Commands
{
    public sealed class AddLanguageSkillCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid ContactId { get; set; }
        public Guid LanguageId { get; set; }
        public int Level { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddLanguageSkillCommand, Result<Guid>>
        {
            private readonly ILanguageSkillRepository languageSkillRepository;

            public CommandHandler(ILanguageSkillRepository languageSkillRepository)
            {
                this.languageSkillRepository = languageSkillRepository;
            }

            public async Task<Result<Guid>> Handle(AddLanguageSkillCommand request, CancellationToken cancellationToken)
            {
                var languageSkill = LanguageSkill.Create(
                    languageId: request.LanguageId,
                    level: request.Level,
                    contactId: request.ContactId
                    );

                await languageSkillRepository.CreateAsync(languageSkill).ConfigureAwait(false);

                return languageSkill.Id;
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddLanguageSkillCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.LanguageId).NotEqual(default(Guid));
                RuleFor(x => x.Level).NotNull().NotEmpty(); // TODO
            }
        }
        #endregion
    }
}
