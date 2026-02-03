using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class TrainingCourse : EntityBase
    {
        public string Name { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Provider { get; set; }
        public Country Country { get; set; }
        public User UserId { get; set; }

        private TrainingCourse()
        {
        }

        public static TrainingCourse Create(
            string trainingCourseName,
            DateTime graduationDate,
            string provider,
            Country country,
            Guid contactId
            )
        {
            Guard.Argument(trainingCourseName, nameof(trainingCourseName)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(provider, nameof(provider)).NotNull().NotEmpty().NotWhiteSpace();
            Guard.Argument(graduationDate, nameof(graduationDate)).NotEqual(default);

            var trainingCourse = new TrainingCourse();
            trainingCourse.Id = Guid.NewGuid();
            trainingCourse.UserId = User.SetId(contactId);
            trainingCourse.Name = trainingCourseName;
            trainingCourse.Provider = provider;
            trainingCourse.GraduationDate = graduationDate;
            trainingCourse.Country = country;
            return trainingCourse;
        }
    }
}
