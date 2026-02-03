using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Mbrcld.Application.Features.EventQuestions.Queries
{
    public sealed class ListEventQuestionByEventIdQuery : IRequest<IList<ListEventQuestionByEventIdViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListEventQuestionByEventIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query Handler
        private sealed class QueryHandler : IRequestHandler<ListEventQuestionByEventIdQuery, IList<ListEventQuestionByEventIdViewModel>>
        {
            private readonly IEventQuestionRepository eventQuestionRepository;
            private readonly IEventRepository eventRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEventQuestionRepository eventQuestionRepository, IEventRepository eventRepository, IMapper mapper)
            {
                this.eventQuestionRepository = eventQuestionRepository;
                this.eventRepository = eventRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListEventQuestionByEventIdViewModel>> Handle(ListEventQuestionByEventIdQuery request, CancellationToken cancellationToken)
            {
                /*var program = await programRepository.GetProgramByIdAsync(request.Id);*/
                var questions = await eventQuestionRepository.ListByEventIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IList<ListEventQuestionByEventIdViewModel>>(questions);
            }
        }
        #endregion
    }
}
