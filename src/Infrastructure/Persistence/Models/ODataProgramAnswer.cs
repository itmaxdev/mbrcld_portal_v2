using AutoMapper;
using Mbrcld.Domain.Entities;
using System;
using System.Runtime.Serialization;

namespace Mbrcld.Infrastructure.Persistence.Models
{
    [DataContract(Name = "do_programanswer")]
    internal sealed class ODataProgramAnswer
    {
        [DataMember(Name = "do_programanswerid")]
        internal Guid Id { get; set; }

        [DataMember(Name = "do_answer")]
        internal string Name { get; set; }

        [DataMember(Name = "do_ProgramQuestionId")]
        internal ODataProgramQuestion QuestionId { get; set; }

        [DataMember(Name = "do_EnrollmentId")]
        internal ODataEnrollment Enrollment { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ODataProgramAnswer, ProgramAnswer>()
                  .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                  .ForMember(dst => dst.AnswerText, x => x.MapFrom(src => src.Name))
                  .ForMember(dst => dst.QuestionId, x => x.MapFrom(src => src.QuestionId.Id))
                  .ForMember(dst => dst.EnrollmentId, x => x.MapFrom(src => src.Enrollment.Id))
                  .ReverseMap();
            }
        }
        #endregion
    }
}
