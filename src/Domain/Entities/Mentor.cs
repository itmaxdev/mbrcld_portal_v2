namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Mentor : EntityBase
    {
        public string Name { get; set; }
        public string AboutMentor { get; set; }
        public string Email { get; set; }
        public Country Nationality { get; set; }
        public string Education { get; set; }
        public string JobPosition { get; set; }
        public string LinkedIn { get; set; }
        public Country ResidenceCountry { get; set; }

        private Mentor() { }
    }
}