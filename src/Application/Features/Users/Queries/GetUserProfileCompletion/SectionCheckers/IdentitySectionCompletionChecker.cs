using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class IdentitySectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {
        internal static readonly DocumentIdentifier[] IdentitySectionDocuments = {
            DocumentIdentifier.IdentityFrontPage,
          //  DocumentIdentifier.IdentityBackPage,
            DocumentIdentifier.PassportFrontPage,
           // DocumentIdentifier.PassportBackPage,
            DocumentIdentifier.FamilyFrontPage,
           // DocumentIdentifier.FamilyBackPage,
        };

        public Task<bool> IsSectionCompleteAsync(User user)
        {
            var result = AllValuesAreNotEmpty(
                user.PassportNumber,
                user.PassportExpiryDate
                );

            if (result && string.Equals(user.Nationality?.IsoCode, "AE", StringComparison.OrdinalIgnoreCase))
            {
                result &= AllValuesAreNotEmpty(
                    user.EmiratesId,
                    user.EmiratesIdExpiryDate,
                    user.PassportIssuingAuthority
                    );
            }

            if (result)
            {
                result &= IdentitySectionDocuments.All(
                    x => user.Documents.Any(
                        y => y.Identifier == x));
            }

            return Task.FromResult(result);
        }

        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(18);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("identity");
    }
}
