using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.EducationQualifications.Commands
{
    public sealed class AddEducationQualificationCommand : IRequest<Result<Guid>>
    {
        #region Command
        public Guid ContactId { get; set; }
        public string University { get; set; }
        public string City { get; set; }
        public string University_AR { get; set; }
        public string Specialization { get; set; }
        public string Specialization_AR { get; set; }
        public int Degree { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Country { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddEducationQualificationCommand, Result<Guid>>
        {
            private readonly IEducationQualificationRepository educationQualificationRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IEducationQualificationRepository educationQualificationRepository, ICountryRepository countryRepository)
            {
                this.educationQualificationRepository = educationQualificationRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result<Guid>> Handle(AddEducationQualificationCommand request, CancellationToken cancellationToken)
            {
                var country = await countryRepository.GetByIsoCodeAsync(request.Country);
                if (country.HasNoValue)
                {
                    throw new Exception($"Invalid country code {request.Country}");
                }

                var educationQualification = EducationQualification.Create(
                    request.ContactId,
                    request.University,
                    request.University_AR,
                    request.Specialization,
                    request.Specialization_AR,
                    request.Degree,
                    request.City,
                    request.GraduationDate,
                    country.Value
                    );

                await educationQualificationRepository.CreateAsync(educationQualification, cancellationToken).ConfigureAwait(false);

                return Result.Success(educationQualification.Id);
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<AddEducationQualificationCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.University).NotNull().NotEmpty();
                RuleFor(x => x.University_AR).NotNull().NotEmpty();
                RuleFor(x => x.Specialization).NotNull().NotEmpty();
                RuleFor(x => x.Specialization_AR).NotNull().NotEmpty();
                RuleFor(x => x.Degree).NotNull().NotEmpty();
                RuleFor(x => x.GraduationDate).NotEqual(default(DateTime));
                RuleFor(x => x.Country).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
