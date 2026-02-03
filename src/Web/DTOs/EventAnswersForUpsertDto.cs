using System;

namespace Mbrcld.Web.DTOs
{
    public sealed class EventAnswersForUpsertDto
    {
        //internal string PictureUrl;

        public string Answer { get; set; }
        public Guid EventQuestionId { get; set; }
    }
}
