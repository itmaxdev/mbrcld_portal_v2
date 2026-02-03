using Mbrcld.Domain.Entities;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class GeneralInformationSectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {
        public Task<bool> IsSectionCompleteAsync(User user)
        {
            var result = AllValuesAreNotEmpty(
                user.FirstName,
                user.FirstNameAr,
                user.MiddleName,
                user.MiddleNameAr,
                user.LastName,
                user.LastNameAr,
                user.BirthDate,
                user.Gender,
                user.Nationality,
                user.ProfilePictureUniqueId
                );

            return Task.FromResult(result);
        }

        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(18);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("general-information");
    }
}
