namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class SystemUser : EntityBase
    {
        public string FullName { get; set; }

        private SystemUser() { }
    }
}