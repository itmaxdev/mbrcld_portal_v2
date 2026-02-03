using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditUserContactDetailsCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string BusinessEmail { get; set; }
        public string MobilePhone { get; set; }
        public string MobilePhone2 { get; set; }
        public string Telephone { get; set; }
        public string ResidenceCountry { get; set; }
        public string City { get; set; }
        public string PostOfficeBox { get; set; }
        public string Address { get; set; }
        public string LinkedInUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<EditUserContactDetailsCommand, Result>
        {
            private readonly IUserRepository userRepository;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IUserRepository userRepository, ICountryRepository countryRepository)
            {
                this.userRepository = userRepository;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(EditUserContactDetailsCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                var country = await countryRepository.GetByIsoCodeAsync(request.ResidenceCountry);
                if (country.HasNoValue)
                {
                    return Result.Failure($"Invalid country code {request.ResidenceCountry}");
                }

                user.BusinessEmail = request.BusinessEmail;
                user.SetMobilePhone(request.MobilePhone);
                user.MobilePhone2 = request.MobilePhone2;
                user.Telephone = request.Telephone;
                user.ResidenceCountry = country.Value;
                user.City = request.City;
                user.PostOfficeBox = request.PostOfficeBox;
                user.Address = request.Address;
                user.LinkedInUrl = request.LinkedInUrl;
                user.InstagramUrl = request.InstagramUrl;
                user.TwitterUrl = request.TwitterUrl;

                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
        #endregion
    }
}
