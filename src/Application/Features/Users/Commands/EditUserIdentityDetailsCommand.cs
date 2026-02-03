using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Commands
{
    public sealed class EditUserIdentityDetailsCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string EmiratesId { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public DateTime? EmiratesIdExpiryDate { get; set; }
        public int? PassportIssuingAuthority { get; set; }

        private sealed class CommandHandler : IRequestHandler<EditUserIdentityDetailsCommand, Result>
        {
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserRepository userRepository)
            {
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(EditUserIdentityDetailsCommand request, CancellationToken cancellationToken)
            {
                var user = (User)await userRepository.GetByIdAsync(request.Id, cancellationToken);

                // Check emiratesId is exist
                var isEmirateIdExist = await userRepository.GetByEmiratesIdAsync(request.EmiratesId, true, request.Id, cancellationToken);
                if (isEmirateIdExist != null)
                {
                    return Result.Failure("Emirates Id already exist.");
                }

                user.EmiratesId = request.EmiratesId;
                user.EmiratesIdExpiryDate = request.EmiratesIdExpiryDate;
                user.PassportNumber = request.PassportNumber;
                user.PassportExpiryDate = request.PassportExpiryDate;
                user.PassportIssuingAuthority = request.PassportIssuingAuthority;
                await userRepository.UpdateAsync(user);

                return Result.Success();
            }
        }
    }
}
