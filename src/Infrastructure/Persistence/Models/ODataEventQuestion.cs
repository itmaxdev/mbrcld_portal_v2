namespace Mbrcld.Infrastructure.Persistence.Models
{
    using AutoMapper;
    using Mbrcld.Domain.Entities;
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "do_eventquestions")]
    internal sealed class ODataEventQuestion
    {
        [DataMember(Name = "do_eventquestionid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_name")]
        internal string NameTest { get; set; }

        [DataMember(Name = "do_name_ar")]
        public string Name_AR { get; set; }

        [DataMember(Name = "do_eventid")]
        internal ODataEvent EventId { get; set; }

        /*        [DataMember(Name = "do_CohortId")]
                internal ODataCohort CohortId { get; set; }*/

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataEventQuestion, EventQuestion>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Name, x => x.MapFrom(src => src.NameTest))
                    .ForMember(dst => dst.EventId, x => x.MapFrom(src => src.EventId.EventId));
            }
        }
        #endregion
    }
}
