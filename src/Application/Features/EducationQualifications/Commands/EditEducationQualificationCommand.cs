using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.EducationQualifications.Commands
{
    public sealed class EditEducationQualificationCommand : IRequest
    {
        #region Command
        public Guid Id { get; set; }
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
        public sealed class CommandHandler : AsyncRequestHandler<EditEducationQualificationCommand>
        {
            private readonly IEducationQualificationRepository educationQualificationRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IEducationQualificationRepository educationQualificationRepository, ICountryRepository countryRepository)
            {
                this.educationQualificationRepository = educationQualificationRepository;
                this.countryRepository = countryRepository;
            }

            protected override async Task Handle(EditEducationQualificationCommand request, CancellationToken cancellationToken)
            {
                var qualification = await educationQualificationRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (qualification.HasNoValue)
                {
                    throw new Exception();
                }

                var country = await countryRepository.GetByIsoCodeAsync(request.Country);
                if (country.HasNoValue)
                {
                    throw new Exception($"Invalid country code {request.Country}");
                }

                var eqValue = qualification.Value;
                eqValue.University = request.University;
                eqValue.University_Ar = request.University_AR;
                eqValue.Specialization = request.Specialization;
                eqValue.Specialization_Ar = request.Specialization_AR;
                eqValue.Degree = request.Degree;
                eqValue.Graduationdate = request.GraduationDate;
                eqValue.Country = country.Value;
                eqValue.City = request.City;

                await educationQualificationRepository.UpdateAsync(eqValue).ConfigureAwait(false);
            }
        }
        #endregion
    }
}
