namespace Mbrcld.Domain.Entities
{
    using Mbrcld.Domain.Common;

    public sealed class Industry : EntityBase
    {
        public string Label { get; private set; }

        private  Industry()
        { }
    }
}
