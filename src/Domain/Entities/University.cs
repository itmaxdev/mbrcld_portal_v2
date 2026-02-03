namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class University : EntityBase
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AboutUniversity { get; set; }
        public Guid? AccessInstructorId { get; set; }
        public string AccessInstructorName { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string POBox { get; set; }
        public string Twitter { get; set; }
        public string Website { get; set; }

        private University() { }

        public void SetCountry(Country country)
        {
            Guard.Argument(country, nameof(country)).NotNull();

            this.Country = country;
        }
    }
}