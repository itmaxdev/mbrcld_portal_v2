using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IUserDocumentService
    {
        Task<Result> InsertOrReplaceDocumentAsync(Guid userId, DocumentIdentifier documentIdentifier, byte[] content, string contentType, string fileName, CancellationToken cancellationToken = default);
        Task<Result> RemoveDocumentAsync(Guid userId, DocumentIdentifier documentIdentifier, CancellationToken cancellationToken = default);
        Task<IList<Document>> ListDocumentsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
