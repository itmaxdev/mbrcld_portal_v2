namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class ApplicantRightPanel : EntityBase
    {


        public string Subject { get; set; }
        public string Description { get; set; }
        public Guid ProgramId { get; set; }
        public Guid ModuleId { get; set; }       

        private ApplicantRightPanel() { }
    }
}