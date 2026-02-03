namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EventAnswer : EntityBase
    {
        public string Answer { get; set; }
        public Guid EventId { get; set; }
        public Guid EventQuestionId { get; set; }
        public Guid EventRegistrantId { get; set; }

        private EventAnswer() { }
        public static EventAnswer Create(string answer, Guid eventid, Guid eventQuestionId, Guid eventRegistrantId)
        {
            Guard.Argument(answer, nameof(answer)).NotEqual(default);
            Guard.Argument(eventid, nameof(eventid)).NotEqual(default);
            Guard.Argument(eventQuestionId, nameof(eventQuestionId)).NotEqual(default);
            Guard.Argument(eventRegistrantId, nameof(eventRegistrantId)).NotEqual(default);


            var eventanswers = new EventAnswer();
            eventanswers.Id = Guid.NewGuid();
            eventanswers.Answer = answer;
            eventanswers.EventId = eventid;
            eventanswers.EventQuestionId = eventQuestionId;
            eventanswers.EventRegistrantId = eventRegistrantId;
            return eventanswers;
        }
    }
}
