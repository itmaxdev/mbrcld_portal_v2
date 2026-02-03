namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;
    using System.Collections.Generic;

    public class SurveyTemplate : EntityBase
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Status { get; set; }

        private SurveyTemplate() { }
    }
}