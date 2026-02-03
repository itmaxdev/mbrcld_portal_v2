using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IAttachedPictureService
    {
        Task<Maybe<byte[]>> GetAttachedPictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetAttachedFileAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetAttachedLargePictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetAttachedCVAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetAttachedSmallPictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<List<string>> GetAttachedFileTypeAsync(Guid key, CancellationToken cancellationToken = default); 
        Task<List<string>> GetAttachedCVTypeAsync(Guid key, CancellationToken cancellationToken = default); 
         Task<Result> AddOrEditArticlePictureAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> AddOrEditProjectIdeaFileAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> AddOrEditSpecialProjectFileAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> AddAttachmentAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetAttachmentAsync(string url, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetRoleAttachmentAsync(string url, CancellationToken cancellationToken = default);
        Task<Result> AddOrEditProgramContentPictureAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> AddOrEditNewsfeedDocumentAsync(Guid id, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> RemoveAttachedDocumentAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
