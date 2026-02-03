using System;

namespace Mbrcld.Domain.Common
{
    public abstract class EntityBase
    {
        public Guid Id { get; protected set; }
    }
}
