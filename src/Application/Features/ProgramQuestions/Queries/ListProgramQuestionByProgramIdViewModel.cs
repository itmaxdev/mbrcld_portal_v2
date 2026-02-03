using AutoMapper;
using Mbrcld.Domain.Entities;
using System;

namespace Mbrcld.Application.Features.ProgramQuestions.Queries
{
    public sealed class ListProgramQuestionByProgramIdViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }

        #region Mapping profile
        private sealed class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ProgramQuestion, ListProgramQuestionByProgramIdViewModel>()
                    .ForMember(dst => dst.Id, x => x.MapFrom(src => src.Id))
                    .ForMember(dst => dst.Question, x => x.MapFrom(src => src.Name));
            }
        }
        #endregion
    }
}
