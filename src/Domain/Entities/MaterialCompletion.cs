namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class MaterialCompletion : EntityBase
    {
        public string Name { get; set; }
        public decimal Completed { get; set; }
        public Guid MaterialId { get; set; }
        public Guid ContactID { get; set; }

        private MaterialCompletion() { }
    }
}