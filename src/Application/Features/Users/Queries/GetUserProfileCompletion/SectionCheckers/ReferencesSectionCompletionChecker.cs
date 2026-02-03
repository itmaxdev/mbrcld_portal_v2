//using Mbrcld.Domain.Entities;
//using System.Threading.Tasks;

//namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
//{
//    internal sealed class ReferencesSectionCompletionChecker
//        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
//    {
//        public Task<bool> IsSectionCompleteAsync(User user)
//            => Task.FromResult(user.References?.Count > 0);

//        public Task<int> GetSectionWeightAsync()
//            => Task.FromResult(1);

//        public Task<string> GetSectionIdentifierAsync()
//            => Task.FromResult("references");
//    }
//}
