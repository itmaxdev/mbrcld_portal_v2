using System.Collections.Generic;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion
{
    public sealed class GetUserProfileCompletionViewModel
    {
        public int CompletionPercentage { get; set; }
        public bool RequiresUpdate { get; set; }
        public IList<KeyValuePair<string, bool>> Sections { get; set; }
    }
}
