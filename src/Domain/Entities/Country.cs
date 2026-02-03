using Mbrcld.Domain.Common;

namespace Mbrcld.Domain.Entities
{
    public sealed class Country : EntityBase
    {
        public string IsoCode { get; private set; }
        public string Name { get;  set; }

        public Country() { }
    }
}
