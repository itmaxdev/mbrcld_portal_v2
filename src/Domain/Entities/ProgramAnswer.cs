using Dawn;
using Mbrcld.Domain.Common;
using System;

namespace Mbrcld.Domain.Entities
{
    public sealed class ProgramAnswer : EntityBase
    {
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
        public Guid EnrollmentId { get; set; }

        private ProgramAnswer() { }

        public static ProgramAnswer Create(
            Guid questionId,
            Guid enrollmentId,
            string answer
            )
        {
            Guard.Argument(questionId, nameof(questionId)).NotEqual(default);
            Guard.Argument(enrollmentId, nameof(enrollmentId)).NotEqual(default);
            Guard.Argument(answer, nameof(answer)).NotNull().NotWhiteSpace();

            var programAnswer = new ProgramAnswer();
            programAnswer.Id = Guid.NewGuid();
            programAnswer.QuestionId = questionId;
            programAnswer.EnrollmentId = enrollmentId;
            programAnswer.AnswerText = answer;
            return programAnswer;
        }
    }
}
