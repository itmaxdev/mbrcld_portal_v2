//using Mbrcld.Domain.Entities;
//using System.Threading.Tasks;

//namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
//{
//    internal sealed class InterestsSectionCompletionChecker
//        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
//    {
//        public Task<bool> IsSectionCompleteAsync(User user)
//            => Task.FromResult(user.Interests?.Count > 0);

//        public Task<int> GetSectionWeightAsync()
//            => Task.FromResult(0);

//        public Task<string> GetSectionIdentifierAsync()
//            => Task.FromResult("skills");
//    }
//}
