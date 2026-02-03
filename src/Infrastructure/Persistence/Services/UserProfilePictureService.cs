using Mbrcld.Application.Interfaces;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Services
{
    internal sealed class UserProfilePictureService : IUserProfilePictureService
    {
        private readonly ISimpleWebApiClient webApiClient;

        public UserProfilePictureService(ISimpleWebApiClient webApiClient)
        {
            this.webApiClient = webApiClient;
        }

        public async Task<Maybe<byte[]>> GetProfilePictureAsync(string key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<UserPictureForRetrieveModel>()
                .Filter(x => x.Key == key)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.ProfilePicture)
                ? Maybe<byte[]>.None
                : model.ProfilePicture;
        }

        public async Task<Maybe<byte[]>> GetUniversityTeamMemberProfilePictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<UniversityTeamMemberPictureForRetrieveModel>()
                .Filter(x => x.TeamMemberId == key)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.ProfilePicture)
                ? Maybe<byte[]>.None
                : model.ProfilePicture;
        }
        public async Task<Maybe<byte[]>> GetSystemUserProfilePictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<SystemUserPictureForRetrieveModel>()
                .Filter(x => x.SystemUserId == key)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.ProfilePicture)
                ? Maybe<byte[]>.None
                : model.ProfilePicture;
        }

        public async Task<Maybe<byte[]>> GetMentorProfilePictureAsync(Guid key, CancellationToken cancellationToken = default)
        {
            var model = await this.webApiClient.For<MentorPictureForRetrieveModel>()
                .Filter(x => x.MentorId == key)
                .ProjectToModel()
                .FindEntryAsync();

            return this.IsArrayNullOrEmpty(model?.ProfilePicture)
                ? Maybe<byte[]>.None
                : model.ProfilePicture;
        }

        public async Task<Result<string>> ChangeProfilePictureAsync(Guid userId, byte[] content, CancellationToken cancellationToken = default)
        {
            var key = Guid.NewGuid().ToString();

            await this.webApiClient.For("contacts")
                .Key(userId)
                .Set(new
                {
                    entityimage = Convert.ToBase64String(content),
                    do_entityimagekey = key,
                })
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success(key);
        }

        public async Task<Result<string>> ChangeTeamMemberProfilePictureAsync(Guid Id, byte[] content, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For("do_universityteammembers")
                .Key(Id)
                .Set(new
                {
                    entityimage = Convert.ToBase64String(content)
                })
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return Result.Success(Id.ToString());
        }

        public async Task<Result> RemoveProfilePictureAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For("contacts")
                .Key(userId)
                .Set(new
                {
                    entityimage = string.Empty,
                    do_entityimagekey = string.Empty,
                })
                .UpdateEntryAsync(false)
                .ConfigureAwait(false);

            return Result.Success();
        }

        private bool IsArrayNullOrEmpty(byte[] array)
        {
            return array == null || array.Length == 0;
        }

        [DataContract(Name = "contacts")]
        private sealed class UserPictureForRetrieveModel
        {
            [DataMember(Name = "contactid")]
            internal Guid ContactId { get; set; }

            [DataMember(Name = "entityimage")]
            internal byte[] ProfilePicture { get; set; }

            [DataMember(Name = "do_entityimagekey")]
            internal string Key { get; set; }
        }

        [DataContract(Name = "do_universityteammembers")]
        private sealed class UniversityTeamMemberPictureForRetrieveModel
        {
            [DataMember(Name = "do_universityteammemberid")]
            internal Guid TeamMemberId { get; set; }

            [DataMember(Name = "entityimage")]
            internal byte[] ProfilePicture { get; set; }
        }

        [DataContract(Name = "systemuser")]
        private sealed class SystemUserPictureForRetrieveModel
        {
            [DataMember(Name = "systemuserid")]
            internal Guid SystemUserId { get; set; }

            [DataMember(Name = "entityimage")]
            internal byte[] ProfilePicture { get; set; }
        }

        [DataContract(Name = "do_mentors")]
        private sealed class MentorPictureForRetrieveModel
        {
            [DataMember(Name = "do_mentorid")]
            internal Guid MentorId { get; set; }

            [DataMember(Name = "entityimage")]
            internal byte[] ProfilePicture { get; set; }
        }
    }
}
