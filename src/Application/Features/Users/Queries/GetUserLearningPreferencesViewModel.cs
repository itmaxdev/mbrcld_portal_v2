namespace Mbrcld.Application.Features.Users.Queries
{
    public sealed class GetUserLearningPreferencesViewModel
    {
        public int[] SelectedValues { get; }

        public GetUserLearningPreferencesViewModel(int[] selectedValues)
        {
            this.SelectedValues = selectedValues;
        }
    }
}
