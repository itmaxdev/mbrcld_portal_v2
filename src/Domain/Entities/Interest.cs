using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public class Interest : EntityBase
    {
        public string Name { get; set; }
        public Guid ContactId { get; set; }

        private Interest()
        { }

        public static Interest Create(
           string name,
           Guid contactId
           )
        {

            Guard.Argument(name, nameof(name)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(contactId, nameof(contactId)).NotEqual(default);

            var skill_Interest = new Interest();
            skill_Interest.Id = Guid.NewGuid();
            skill_Interest.Name = name;
            skill_Interest.ContactId = contactId;
            return skill_Interest;
        }
    }
}
