using Mbrcld.Domain.Common;
using Mbrcld.Domain.ValueObjects;

namespace Mbrcld.Domain.Entities
{
    public sealed class Document : EntityBase
    {
        public DocumentIdentifier Identifier { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }

        private Document() { }
    }
}
