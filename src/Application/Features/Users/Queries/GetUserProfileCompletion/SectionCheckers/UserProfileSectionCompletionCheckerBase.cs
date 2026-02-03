using System.Collections.Generic;
using System.Linq;

namespace Mbrcld.Application.Features.Users.Queries.GetUserProfileCompletion.SectionCheckers
{
    internal abstract class UserProfileSectionCompletionCheckerBase
    {
        protected bool AllValuesAreNotEmpty(params dynamic[] values)
        {
            bool isAnyValueEmpty = false;
            for (var i = 0; !isAnyValueEmpty && i < values.Length; ++i)
            {
                isAnyValueEmpty = values[i] is null || IsValueEmpty(values[i]);
            }
            return !isAnyValueEmpty;
        }

        protected bool AnyValueIsNotEmpty(params dynamic[] values)
        {
            bool areAllValuesEmpty = true;
            for (var i = 0; areAllValuesEmpty && i < values.Length; ++i)
            {
                areAllValuesEmpty &= values[i] is null || IsValueEmpty(values[i]);
            }
            return !areAllValuesEmpty;
        }

        private bool IsValueEmpty(IEnumerable<dynamic> enumerable)
        {
            return enumerable?.Count() == 0;
        }

        private bool IsValueEmpty(string stringValue)
        {
            return string.IsNullOrEmpty(stringValue);
        }

        private bool IsValueEmpty(object obj)
        {
            return obj == default;
        }
    }
}
