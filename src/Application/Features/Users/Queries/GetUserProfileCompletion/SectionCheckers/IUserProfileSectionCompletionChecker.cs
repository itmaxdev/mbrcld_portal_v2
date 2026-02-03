using Mbrcld.Domain.Entities;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal interface IUserProfileSectionCompletionChecker
    {
        Task<bool> IsSectionCompleteAsync(User user);
        Task<int> GetSectionWeightAsync();
        Task<string> GetSectionIdentifierAsync();
    }
}
