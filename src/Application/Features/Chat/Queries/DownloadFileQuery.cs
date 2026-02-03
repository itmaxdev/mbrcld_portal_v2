using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Chat.Queries
{
    public class DownloadFileQuery : IRequest<DownloadFileViewModel>
    {
        public string Url { get; set; }
        public Guid RoomId { get; set; }

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<DownloadFileQuery, DownloadFileViewModel>
        {
            private readonly IUploadFileService fileService;
            private readonly IChatRepository chatRepository;
            private readonly IMapper mapper;

            public QueryHandler(IUploadFileService fileService, IChatRepository chatRepository, IMapper mapper)
            {
                this.fileService = fileService;
                this.chatRepository = chatRepository;
                this.mapper = mapper;
            }

            public async Task<DownloadFileViewModel> Handle(DownloadFileQuery request, CancellationToken cancellationToken)
            {
                var file = await fileService.Download(request.Url, request.RoomId, cancellationToken);

                var fileInfo = await chatRepository.GetFileByUrl(request.Url, cancellationToken);

                var viewModel = mapper.Map<DownloadFileViewModel>(fileInfo);
                viewModel.File = file.Value;

                return viewModel;
            }
        }
        #endregion
    }
}
