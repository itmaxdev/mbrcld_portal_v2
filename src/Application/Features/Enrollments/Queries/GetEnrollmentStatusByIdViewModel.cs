using System;
using System.Collections.Generic;
using System.Text;

namespace Mbrcld.Application.Features.Enrollments.Queries
{
   public sealed class GetEnrollmentStatusByIdViewModel
    {
        public bool IsAchievementStepCompleted { get; set; }
        public bool IsReferenceStepCompleted { get; set; }
        public bool IsQuestionStepCompleted { get; set; }
        public bool IsSmartAssessmentStepCompleted { get; set; }
        public bool IsAcknowledgmentStepCompleted { get; set; }

    }
}
