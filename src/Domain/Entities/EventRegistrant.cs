namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class EventRegistrant : EntityBase
    {
        public string Name { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public EventRegistrantStatus StatusCode { get; set; }

        private EventRegistrant() { }
        public static EventRegistrant Create(
            Guid userid,
            Guid eventid
            )
        {
            Guard.Argument(userid, nameof(userid)).NotEqual(default);
            Guard.Argument(eventid, nameof(eventid)).NotEqual(default);

            var eventregistrant = new EventRegistrant();
            eventregistrant.Id = Guid.NewGuid();
            eventregistrant.UserId = userid;
            eventregistrant.EventId = eventid;
            return eventregistrant;
        }
        public enum EventRegistrantStatus
        {
            UnderReview = 1,
            Accepted = 936510000,
            Rejected = 936510001
        }
    }
}