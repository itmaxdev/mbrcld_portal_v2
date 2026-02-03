using AutoMapper;
using ChatData.Models;
using FluentValidation;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Commands
{
    public class CreateRoomCommand : IRequest<Result<Room>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Guid ModuleId { get; set; }
        public Guid InstructorId { get; set; }
        public List<Guid> Participants { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<CreateRoomCommand, Result<Room>>
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

            public async Task<Result<Room>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
            {
                var room = new Room();
                request.Participants.Add(request.InstructorId);

                var roomExist = await chatRepository.IsRoomExist(request.Participants, request.ModuleId, cancellationToken);

                if (roomExist != null)
                    return roomExist;

                var users = new List<User>();

                foreach (var id in request.Participants)
                {
                    var user = await userRepository.GetByIdAsync(id, cancellationToken);
                    users.Add(user.Value);
                }

                var roomParticipants = users.Select(p => mapper.Map<Participants>(p)).ToList();

                foreach (var particpant in roomParticipants)
                {
                    particpant.Id = Guid.NewGuid();
                    particpant.RoomId = room.Id;
                }

                room.Participants = roomParticipants;
                room.Name = request.Name;
                room.Description = request.Description;
                room.Image = request.Image;
                room.ModuleId = request.ModuleId;

                return await chatRepository.CreateRoom(room, cancellationToken);
            }
        }
        #endregion

        #region Command validator
        public class CommandValidator : AbstractValidator<CreateRoomCommand>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Participants).NotNull().NotEmpty();
                RuleFor(x => x.ModuleId).NotEmpty();
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
