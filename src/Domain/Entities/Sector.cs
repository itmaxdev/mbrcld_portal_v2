namespace Mbrcld.Domain.Entities
{
    using Mbrcld.Domain.Common;

    public sealed class Sector : EntityBase
    {
        public string Label { get; private set; }

        private Sector()
        { }
    }
}
