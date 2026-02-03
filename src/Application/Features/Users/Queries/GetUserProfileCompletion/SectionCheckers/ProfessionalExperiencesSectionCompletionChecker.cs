using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class ProfessionalExperiencesSectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {

        public Task<bool> IsSectionCompleteAsync(User user)
            => Task.FromResult(
                user.ProfessionalExperiences?.Count > 0 &&
                user.ProfessionalExperiences.Where(c=>c.PositionLevel==null || c.OrganizationLevel==null).Count()==0 &&
                user.Documents.Any(e => e.Identifier == DocumentIdentifier.CurriculumVitae)
                );

        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(18);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("professional-experience");
    }
}
