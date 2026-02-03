using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditUserGeneralInformationCommand : IRequest<Result>
    {
        #region Command
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string FirstName_AR { get; set; }
        public string MiddleName { get; set; }
        public string MiddleName_AR { get; set; }
        public string LastName { get; set; }
        public string LastName_AR { get; set; }
        public int? Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? MaritalStatus { get; set; }
        public string Telephone { get; set; }
        public int? Salutation { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditUserGeneralInformationCommand, Result>
        {
            private readonly IUserRepository userRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IUserRepository userRepository, ICountryRepository countryRepository)
            {
                this.userRepository = userRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(EditUserGeneralInformationCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                var country = await countryRepository.GetByIsoCodeAsync(request.Nationality);
                if (country.HasNoValue)
                {
                    return Result.Failure($"Invalid country code {request.Nationality}");
                }

                user.FirstName = request.FirstName;
                user.FirstNameAr = request.FirstName_AR;
                user.MiddleName = request.MiddleName;
                user.MiddleNameAr = request.MiddleName_AR;
                user.LastName = request.LastName;
                user.LastNameAr = request.LastName_AR;
                user.Gender = request.Gender.HasValue ? (Gender)request.Gender : null;
                user.SetNationality(country.Value);
                user.BirthDate = request.Birthdate;
                user.MaritalStatus = request.MaritalStatus;
                user.Telephone = request.Telephone;

                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
        #endregion
    }
}
