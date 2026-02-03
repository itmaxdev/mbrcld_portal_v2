using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.ProgramAnswers.Queries
{
    public sealed class ListProgramAnswersByEnrollmentIdViewModel
    {
        public Guid QuestionId { get; set; }
        public string AnswerText { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ProgramAnswer, ListProgramAnswersByEnrollmentIdViewModel>()
                    .ForMember(dst => dst.QuestionId, x => x.MapFrom(src => src.QuestionId))
                    .ForMember(dst => dst.AnswerText, x => x.MapFrom(src => src.AnswerText));
            }
        }
        #endregion
    }
}
