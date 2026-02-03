using Mbrcld.Domain.Entities;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal sealed class TrainingCoursesSectionCompletionChecker
        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
    {
        public Task<bool> IsSectionCompleteAsync(User user)
            => Task.FromResult(user.TrainingCourses?.Count > 0);

        public Task<int> GetSectionWeightAsync()
            => Task.FromResult(7);

        public Task<string> GetSectionIdentifierAsync()
            => Task.FromResult("training");
    }
}
