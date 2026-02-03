using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IUploadFileService
    {
        Task<Result<string>> Upload(Guid fileId, Guid roomId, Guid messageId, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> Download(string url, Guid roomId, CancellationToken cancellationToken = default);
    }
}
