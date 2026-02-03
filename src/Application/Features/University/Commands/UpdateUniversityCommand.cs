using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Universities.Commands
{
    public sealed class UpdateUniversityCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AboutUniversity { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string POBox { get; set; }
        public string Twitter { get; set; }
        public string Website { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<UpdateUniversityCommand, Result>
        {
            private readonly IUniversityRepository universityRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IUniversityRepository universityRepository, ICountryRepository countryRepository)
            {
                this.universityRepository = universityRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
            {
                var university = await universityRepository.GetUniversityByIdAsync(request.Id, cancellationToken);
                if (university.HasNoValue)
                {
                    return Result.Failure($"Invalid University with ID { request.Id}");
                }

                var country = await countryRepository.GetByIsoCodeAsync(request.Country);
                if (country.HasNoValue)
                {
                    return Result.Failure($"Invalid country code {request.Country}");
                }

                var universityValue = university.Value;
                universityValue.Name = request.Name;
                universityValue.Phone = request.Phone;
                universityValue.Email = request.Email;
                universityValue.AboutUniversity = request.AboutUniversity;
                universityValue.City = request.City;
                universityValue.SetCountry(country.Value);
                universityValue.Address = request.Address;
                universityValue.Instagram = request.Instagram;
                universityValue.LinkedIn = request.LinkedIn;
                universityValue.POBox = request.POBox;
                universityValue.Twitter = request.Twitter;
                universityValue.Website = request.Website;

                await universityRepository.UpdateAsync(universityValue).ConfigureAwait(false);

                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public sealed class CommandValidator : AbstractValidator<UpdateUniversityCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty();
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
