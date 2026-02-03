using ChatData.Models;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Commands
{
    public class ChatUploadCommand : IRequest<Result<string>>
    {
        public Guid RoomId { get; set; }
        public Guid FileId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }

        #region Command handler
        private sealed class CommandHandler : IRequestHandler<ChatUploadCommand, Result<string>>
        {
            private readonly IUploadFileService fileService;
            private readonly IChatRepository chatRepository;

            public CommandHandler(IUploadFileService fileService, IChatRepository chatRepository)
            {
                this.fileService = fileService;
                this.chatRepository = chatRepository;
            }

            public async Task<Result<string>> Handle(ChatUploadCommand request, CancellationToken cancellationToken)
            {
                var message = new Message();
                message.Id = Guid.NewGuid();

                message.RoomId = request.RoomId;
                message.MessageType = MessageTypes.File;
                message.FileId = request.FileId;
                message.UserId = request.UserId;
                message.Text = request.FileName;

                await chatRepository.SendMessage(message, cancellationToken);

                return await fileService.Upload(request.FileId,
                                                request.RoomId,
                                                message.Id,
                                                request.File,
                                                request.ContentType,
                                                request.FileName,
                                                cancellationToken);
            }
        }
        #endregion

    }
}
