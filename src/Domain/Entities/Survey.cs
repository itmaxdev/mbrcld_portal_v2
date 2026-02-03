namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class Survey : EntityBase
    {
        public string Name { get; set; }
        public string SurveyTemplateName { get; set; }
        public string SurveyURL { get; set; }
        public DateTime Date { get; set; }
        public Guid SurveyTemplateId { get; set; }
        public Guid ContactId { get; set; }
        public Guid ProgramId { get; set; }
        public int Status { get; set; }

        private Survey() { }
    }
}