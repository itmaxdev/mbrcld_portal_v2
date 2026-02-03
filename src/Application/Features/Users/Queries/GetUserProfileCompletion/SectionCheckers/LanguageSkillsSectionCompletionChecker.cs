//using Mbrcld.Domain.Entities;
//using System.Threading.Tasks;

//namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
//{
//    internal sealed class LanguageSkillsSectionCompletionChecker
//        : UserProfileSectionCompletionCheckerBase, IUserProfileSectionCompletionChecker
//    {
//        public Task<bool> IsSectionCompleteAsync(User user)
//            => Task.FromResult(user.LanguageSkills?.Count > 0);

//        public Task<int> GetSectionWeightAsync()
//            => Task.FromResult(0);

//        public Task<string> GetSectionIdentifierAsync()
//            => Task.FromResult("languages");
//    }
//}
