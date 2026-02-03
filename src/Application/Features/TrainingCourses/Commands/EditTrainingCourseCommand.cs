using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TrainingCourses.Commands
{
    public sealed class EditTrainingCourseCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Provider { get; set; }
        public string Country { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditTrainingCourseCommand, Result>
        {
            private readonly ITrainingCourseRepository trainingCourseRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(ITrainingCourseRepository trainingCourseRepository, ICountryRepository countryRepository)
            {
                this.trainingCourseRepository = trainingCourseRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(EditTrainingCourseCommand request, CancellationToken cancellationToken)
            {
                var country = await countryRepository.GetByIsoCodeAsync(request.Country, cancellationToken);
                if (country.HasNoValue)
                {
                    return Result.Failure($"Invalid country code {request.Country}");
                }

                var tcPull = await trainingCourseRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (tcPull.HasNoValue)
                {
                    throw new Exception();
                }

                var tcValue = tcPull.Value;

                tcValue.Name = request.Name;
                tcValue.Provider = request.Provider;
                tcValue.GraduationDate = request.GraduationDate;
                tcValue.Country = country.Value;

                await trainingCourseRepository.UpdateAsync(tcValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public class UpdateTCCommandValidator : AbstractValidator<EditTrainingCourseCommand>
        {
            public UpdateTCCommandValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Provider).NotNull().NotEmpty();
                RuleFor(x => x.GraduationDate).NotNull().NotEmpty();
                RuleFor(x => x.Country).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
