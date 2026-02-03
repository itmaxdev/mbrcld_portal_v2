using AutoMapper;
using ChatData.Models;
using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Commands
{
    public class AddUserToRoomCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<AddUserToRoomCommand, Result>
        {
            private readonly IChatRepository chatRepository;
            private readonly IUserRepository userRepository;
            private readonly IMapper mapper;

            public CommandHandler(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
            {
                this.chatRepository = chatRepository;
                this.userRepository = userRepository;
                this.mapper = mapper;
            }

            public async Task<Result> Handle(AddUserToRoomCommand request, CancellationToken cancellationToken)
            {
                var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
                await chatRepository.AddParticipant(user.Value, request.RoomId, cancellationToken);
                return Result.Success();
            }
        }
        #endregion

        #region Command validator
        public class CommandValidator : AbstractValidator<AddUserToRoomCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RoomId).NotNull().NotEmpty();
                RuleFor(x => x.UserId).NotEmpty();
            }
        }
        #endregion

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<User, Participants>()
                    .ForMember(dst => dst.UserId, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.FullName, x => x.MapFrom(src => src.FirstName + " " + src.MiddleName + " " + src.LastName))
                    .ForMember(dst => dst.FullNameAr, x => x.MapFrom(src => src.FirstNameAr + " " + src.MiddleNameAr + " " + src.LastNameAr))
                    .ForMember(dst => dst.UserAvater, x => x.MapFrom(src => src.ProfilePictureUniqueId));
            }
        }
        #endregion
    }
}
