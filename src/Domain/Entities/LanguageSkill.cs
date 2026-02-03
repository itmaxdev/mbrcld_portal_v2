namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class LanguageSkill : EntityBase
    {   
        public Guid LanguageId { get; set; }
        public int Level { get; set; }
        public Guid ContactId { get; set; }

        private LanguageSkill()
        { }

        public static LanguageSkill Create(
           Guid languageId,
           int level,
           Guid contactId
           )
        {
           
            Guard.Argument(languageId, nameof(languageId)).NotEqual(default);
            Guard.Argument(level, nameof(level)).NotEqual(default);
            Guard.Argument(contactId, nameof(contactId)).NotEqual(default);

            var languageSkill = new LanguageSkill();
            languageSkill.Id = Guid.NewGuid();
            languageSkill.LanguageId = languageId;
            languageSkill.Level = level;
            languageSkill.ContactId = contactId;
            return languageSkill;
        }
    }
}
