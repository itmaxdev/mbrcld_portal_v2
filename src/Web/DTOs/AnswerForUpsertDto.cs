using System;

namespace Mbrcld.Web.DTOs
{
    public sealed class AnswerForUpsertDto
    {
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }
    }
}
