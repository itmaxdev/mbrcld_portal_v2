using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_newsfeed")]
    internal sealed class ODataNewsFeed
    {
        [DataMember(Name = "do_newsfeedid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_ModuleId")]
        internal ODataModule Module { get; set; }

        [DataMember(Name = "do_InstructorId")]
        internal ODataContact Instructor { get; set; }

        [DataMember(Name = "do_name")]
        internal string Name { get; set; }

        [DataMember(Name = "do_duration")]
        internal int Duration { get; set; }

        [DataMember(Name = "do_order")]
        internal decimal Order { get; set; }

        [DataMember(Name = "do_meetingstartdate")]
        internal DateTime? MeetingStartDate { get; set; }

        [DataMember(Name = "do_expirydate")]
        internal DateTime? ExpiryDate { get; set; }

        [DataMember(Name = "do_publishdate")]
        internal DateTime? PublishDate { get; set; }

        [DataMember(Name = "do_type")]
        internal int Type { get; set; }

        [DataMember(Name = "statuscode")]
        internal int Status { get; set; }

        [DataMember(Name = "do_notifyusers")]
        internal bool NotifyUsers { get; set; }

        [DataMember(Name = "do_url")]
        internal string Url { get; set; }

        [DataMember(Name = "do_text")]
        internal string Text { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataNewsFeed, NewsFeed>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.Name, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.ModuleId, x => x.MapFrom(src => src.Module.Id))
                  .ForMember(dst => dst.ModuleName, x => x.MapFrom(src => src.Module.Name))
                  .ForMember(dst => dst.InstructorId, x => x.MapFrom(src => src.Instructor.ContactId))
                  .ForMember(dst => dst.InstructorName, x => x.MapFrom(src => src.Instructor.FirstName + " " + src.Instructor.MiddleName + " " + src.Instructor.LastName))
                  .ForMember(dst => dst.Duration, x => x.MapFrom(src => src.Duration))
                  .ForMember(dst => dst.Order, x => x.MapFrom(src => src.Order))
                  .ForMember(dst => dst.MeetingStartDate, x => x.MapFrom(src => src.MeetingStartDate))
                  .ForMember(dst => dst.ExpiryDate, x => x.MapFrom(src => src.ExpiryDate))
                  .ForMember(dst => dst.PublishDate, x => x.MapFrom(src => src.PublishDate))
                  .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                  .ForMember(dst => dst.Status, x => x.MapFrom(src => src.Status))
                  .ForMember(dst => dst.NotifyUsers, x => x.MapFrom(src => src.NotifyUsers))
                  .ForMember(dst => dst.Text, x => x.MapFrom(src => src.Text))
                  .ForMember(dst => dst.Url, x => x.MapFrom(src => src.Url))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
