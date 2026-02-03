using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class Membership : EntityBase
    {
        public string InstitutionName { get; set; }
        public string InstitutionName_AR { get; set; }
        public DateTime JoinDate { get; set; }
        public string RoleName { get; set; }
        public string RoleName_AR { get; set; }
        public bool Active { get; set; }
        public User UserId { get; set; }
        public int? MembershipLevel { get; set; }
        public bool Completed { get; set; }
        private Membership()
        {
        }

        public static Membership Create(
           string institutionName,
           string institutionName_AR,
           DateTime joinDate,
           string roleName,
           string roleName_AR,
           bool active,
           Guid contactId,
           int? membershipLevel
           
           )
        {
            Guard.Argument(institutionName, nameof(institutionName)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(institutionName_AR, nameof(institutionName_AR)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(joinDate, nameof(joinDate)).NotEqual(default);
            Guard.Argument(roleName, nameof(roleName)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(roleName_AR, nameof(roleName_AR)).NotNull().NotEmpty().NotWhiteSpace();
           // Guard.Argument(active, nameof(active)).NotEqual(default);
            Guard.Argument(contactId, nameof(contactId)).NotEqual(default);
          //  Guard.Argument(membershipLevel, nameof(membershipLevel)).NotNull().NotEmpty().NotWhiteSpace();

            var membership = new Membership();
            membership.Id = Guid.NewGuid();
            membership.UserId = User.SetId(contactId);
            membership.InstitutionName = institutionName;
            membership.InstitutionName_AR = institutionName_AR;
            membership.JoinDate = joinDate;
            membership.RoleName = roleName;
            membership.RoleName_AR = roleName_AR;
            membership.Active = active;
            membership.MembershipLevel = membershipLevel;
            return membership;
        }
    }
}
