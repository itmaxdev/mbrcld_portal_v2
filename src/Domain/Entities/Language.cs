
namespace Mbrcld.Domain.Entities
{
    using Mbrcld.Domain.Common;
    public sealed class Language : EntityBase
    {
        public string Label { get; private set; }

        private Language()
        { }
    }
}
