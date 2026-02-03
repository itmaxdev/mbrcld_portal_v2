namespace Mbrcld.Domain.Entities
{
    using Dawn;
    using Mbrcld.Domain.Common;
    using System;

    public class Content : EntityBase
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public decimal Order { get; set; }
        public DateTime? StartDate { get; set; }
        public Guid SectionId { get; set; }

        private Content() { }
        public static Content Create(
            Guid sectionid,
            string name,
            int duration,
            decimal order,
            string text,
            int type,
            string url,
            DateTime? startdate
            )
        {
            Guard.Argument(sectionid, nameof(sectionid)).NotEqual(default);
            Guard.Argument(type, nameof(type)).NotEqual(default);

            var content = new Content();
            content.Id = Guid.NewGuid();
            content.Name = name;
            content.Duration = duration;
            content.Order = order;
            content.Text = text;
            content.Type = type;
            content.Url = url;
            content.SectionId = sectionid;
            content.StartDate = startdate;
            return content;
        }
    }
}