using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Interfaces
{
    public interface IUserProfilePictureService
    {
        Task<Maybe<byte[]>> GetProfilePictureAsync(string key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetUniversityTeamMemberProfilePictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetSystemUserProfilePictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Maybe<byte[]>> GetMentorProfilePictureAsync(Guid key, CancellationToken cancellationToken = default);
        Task<Result<string>> ChangeProfilePictureAsync(Guid userId, byte[] content, CancellationToken cancellationToken = default);
        Task<Result<string>> ChangeTeamMemberProfilePictureAsync(Guid Id, byte[] content, CancellationToken cancellationToken = default);
        Task<Result> RemoveProfilePictureAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
