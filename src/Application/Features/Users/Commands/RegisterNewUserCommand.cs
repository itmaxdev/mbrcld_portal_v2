using FluentValidation;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class RegisterNewUserCommand : IRequest<Result>
    {
        #region Command
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Password { get; set; }
        public string EmiratesId { get; set; }
        public bool IsUAELogin { get; set; }
        #endregion

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<RegisterNewUserCommand, Result>
        {
            private readonly IAccountService accountService;
            private readonly ICountryRepository countryRepository;

            public CommandHandler(IAccountService accountService, ICountryRepository countryRepository)
            {
                this.accountService = accountService;
                this.countryRepository = countryRepository;
            }

            public async Task<Result> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
            {
                var nationality = await countryRepository.GetByIsoCodeAsync(request.Nationality);
                if (nationality.HasNoValue)
                {
                    return Result.Failure($"Invalid nationality '{request.Nationality}'");
                }

                var email = SharedKernel.ValueObjects.Email.Create(request.Email);
                if (email.HasNoValue)
                {
                    return Result.Failure("Invalid email");
                }

                var user = User.Create(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: email.Value,
                    mobilePhone: request.MobilePhone,
                    nationality: nationality.Value,
                    role: 1, //Registrant
                    emailConfirmed: request.IsUAELogin
                    );

                user.EmiratesId = request.EmiratesId;

                var result = await accountService.CreateAsync(user, request.Password);

                return result;
            }
        }
        #endregion

        #region Command Validator
        public sealed class CommandValidator : AbstractValidator<RegisterNewUserCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.MobilePhone).NotEmpty();
                RuleFor(x => x.Nationality).NotEmpty();
                RuleFor(x => x.EmiratesId).NotNull().NotEmpty()
                    .When(x => x.Nationality?.ToUpper() == "AE");
            }
        }
        #endregion
    }
}
