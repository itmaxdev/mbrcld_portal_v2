using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.LanguageSkills.Commands
{
    public sealed class EditLanguageSkillCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public Guid LanguageId { get; set; }
        public int Level { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditLanguageSkillCommand, Result>
        {
            private readonly ILanguageSkillRepository languageSkillRepository;

            public CommandHandler(ILanguageSkillRepository languageSkillRepository)
            {
                this.languageSkillRepository = languageSkillRepository;
            }

            public async Task<Result> Handle(EditLanguageSkillCommand request, CancellationToken cancellationToken)
            {
                var languageSkillPull = await languageSkillRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (languageSkillPull.HasNoValue)
                {
                    return Result.Failure();
                }

                var languageSkillValue = languageSkillPull.Value;
                languageSkillValue.LanguageId = request.LanguageId;
                languageSkillValue.Level = request.Level;

                await languageSkillRepository.UpdateAsync(languageSkillValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<EditLanguageSkillCommand>
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
