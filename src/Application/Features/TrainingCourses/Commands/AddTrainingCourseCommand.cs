using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TrainingCourses.Commands
{
    public sealed class AddTrainingCourseCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Provider { get; set; }
        public string Country { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddTrainingCourseCommand, Result<Guid>>
        {
            private readonly ITrainingCourseRepository trainingCourseRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(ITrainingCourseRepository trainingCourseRepository, ICountryRepository countryRepository)
            {
                this.trainingCourseRepository = trainingCourseRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result<Guid>> Handle(AddTrainingCourseCommand request, CancellationToken cancellationToken)
            {
                var country = await countryRepository.GetByIsoCodeAsync(request.Country, cancellationToken);
                if (country.HasNoValue)
                {
                    return Result.Failure<Guid>($"Invalid country code {request.Country}");
                }

                var trainingCourse = TrainingCourse.Create(
                    trainingCourseName: request.Name,
                    graduationDate: request.GraduationDate,
                    provider: request.Provider,
                    country: country.Value,
                    contactId: request.ContactId
                    );

                await trainingCourseRepository.CreateAsync(trainingCourse).ConfigureAwait(false);

                return Result.Success(trainingCourse.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddTrainingCourseCommand>
        {
            public CommandValidator()
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
