using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class Role : EntityBase
    {
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }

        private Role() { }

        public void SetName(string name)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotEmpty().NotWhiteSpace();
            
            this.Name = name;
        }

        public void SetNormalizedName(string normalizedName)
        {
            Guard.Argument(normalizedName, nameof(normalizedName)).NotNull().NotEmpty().NotWhiteSpace();

            this.NormalizedName = normalizedName;
        }

        public static Role Create(string name)
        {
            Guard.Argument(name, nameof(name)).NotNull().NotEmpty().NotWhiteSpace();

            return new Role()
            {
                Id = Guid.NewGuid(),
                Name = name,
            };
        }
    }
}
