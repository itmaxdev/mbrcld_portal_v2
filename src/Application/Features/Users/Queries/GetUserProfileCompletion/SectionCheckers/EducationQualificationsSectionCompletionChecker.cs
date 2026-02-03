using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class EducationQualificationsSectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {
        public Task<bool> IsSectionCompleteAsync(User user)
            => Task.FromResult(
                user.EducationQualifications?.Count > 0 &&
                user.Documents.Any(e => e.Identifier == DocumentIdentifier.EducationCertificate)
                );
        
        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(15);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("education");
    }
}
