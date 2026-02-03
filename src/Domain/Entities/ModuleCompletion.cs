namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class ModuleCompletion : EntityBase
    {
        public string Name { get; set; }
        public decimal Completed { get; set; }
        public Guid ModuleId { get; set; }
        public Guid ContactID { get; set; }

        private ModuleCompletion() { }
    }
}