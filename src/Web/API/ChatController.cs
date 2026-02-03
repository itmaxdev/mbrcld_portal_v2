using ChatData.Models;
using Mbrcld.Application.Features.Chat.Commands;
using Mbrcld.Application.Features.Chat.Queries;
using Mbrcld.SharedKernel.Result;
using Mbrcld.Web.Constants;
using Mbrcld.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mbrcld.Web.API
{
    [Authorize]
    [ApiController]
    [ApiVersion(ApiVersionConstants.Version_1_0)]
    [Route("api/chat")]
    public class ChatController : BaseController
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{moduleId}")]
        public async Task<IActionResult> GetUserRooms([FromRoute] Guid moduleId)
        {
            var userId = User.GetUserId();
            var rooms = await _mediator.Send(new GetUserRoomsQuery { UserId = userId, ModuleId = moduleId });

            return Ok(rooms);
        }

        [HttpGet("meeting-rooms")]
        public async Task<IActionResult> GetMeetingHubRooms()
        {
            var rooms = await _mediator.Send(new GetMeetingHubRoomsQuery());

            return Ok(rooms);
        }

        [HttpGet("{excludeUserId}/rooms")]
        public async Task<IActionResult> GetUserInvolvedRooms([FromRoute] Guid excludeUserId)
        {
            var currentUserId = User.GetUserId();
            var rooms = await _mediator.Send(new GetCurrentUserRoomsQuery { CurrentUserId = currentUserId, ExcludeUserId = excludeUserId });
            return Ok(rooms);
        }

        [HttpGet("{roomId}/messages")]
        public async Task<IActionResult> GetRoomMessages([FromRoute] Guid roomId, [FromQuery] MessageTypes? type)
        {
            var messages = await _mediator.Send(new GetRoomMessagesQuery { RoomId = roomId, MessageType = type });

            return Ok(messages);
        }

        [Authorize(Roles = "Instructor, Applicant, Alumni")]
        [HttpPost("room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
        {
            command.InstructorId = User.GetUserId();

            var room = await _mediator.Send(command);

            return Ok(room);
        }

        [HttpPost("{userId}/room/{roomId}")]
        public async Task<IActionResult> AddUserToRoom([FromRoute] Guid userId, [FromRoute] Guid roomId)
        {
            var userToRoom = new AddUserToRoomCommand { UserId = userId, RoomId = roomId };
            await _mediator.Send(userToRoom);
            return Ok();
        }

        [HttpPost("{roomId}/upload")]
        public async Task<IActionResult> ChatUpload([FromRoute] Guid roomId, IFormFile file)
        {
            using var ms = new MemoryStream();
            var userId = User.GetUserId();

            if (file != null)
            {
                file.CopyTo(ms);
                var command = new ChatUploadCommand
                {
                    RoomId = roomId,
                    File = ms.ToArray(),
                    UserId = userId,
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }

            return FromResult(Result.Failure("No File Uploaded"));
        }

        [HttpGet("{roomId}/download")]
        public async Task<IActionResult> Download([FromRoute] Guid roomId, [FromQuery] string url)
        {
            var result = await _mediator.Send(new DownloadFileQuery { Url = url, RoomId = roomId });
            
            return File(result.File, result.ContentType, result.FileName, false);
        }

        [HttpDelete("{userId}/room/{roomId}")]
        public async Task<IActionResult> RemoveUserFromRoom([FromRoute] Guid userId, [FromRoute] Guid roomId)
        {
            var removeUserFromRoom = new RemoveUserFromRoomCommand { UserId = userId, RoomId = roomId };
            await _mediator.Send(removeUserFromRoom);
            return Ok();
        }
    }
}
