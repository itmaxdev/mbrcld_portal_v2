using Mbrcld.Domain.Entities;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class ContactDetailsSectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {
        public Task<bool> IsSectionCompleteAsync(User user)
        {
            var result = AllValuesAreNotEmpty(
                user.ResidenceCountry,
                user.Email,
                user.BusinessEmail,
                user.MobilePhone,
                user.City,
                user.Address
                );

            result &= AnyValueIsNotEmpty(
                user.InstagramUrl,
                user.TwitterUrl,
                user.LinkedInUrl
                );

            return Task.FromResult(result);
        }

        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(13);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("contact-details");
    }
}
