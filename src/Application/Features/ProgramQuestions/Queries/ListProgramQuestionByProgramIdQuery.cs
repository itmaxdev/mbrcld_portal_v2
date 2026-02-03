using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProgramQuestions.Queries
{
    public sealed class ListProgramQuestionByProgramIdQuery : IRequest<IList<ListProgramQuestionByProgramIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListProgramQuestionByProgramIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListProgramQuestionByProgramIdQuery, IList<ListProgramQuestionByProgramIdViewModel>>
        {
            private readonly IProgramQuestionRepository programQuestionRepository;
            private readonly IProgramRepository programRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProgramQuestionRepository programQuestionRepository, IProgramRepository programRepository, IMapper mapper)
            {
                this.programQuestionRepository = programQuestionRepository;
                this.programRepository = programRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListProgramQuestionByProgramIdViewModel>> Handle(ListProgramQuestionByProgramIdQuery request, CancellationToken cancellationToken)
            {
                var program = await programRepository.GetProgramByIdAsync(request.Id);
                var questions = await programQuestionRepository.ListByProgramIdAsync(program.CohortId).ConfigureAwait(false);
                return mapper.Map<IList<ListProgramQuestionByProgramIdViewModel>>(questions);
            }
        }
        #endregion
    }
}
