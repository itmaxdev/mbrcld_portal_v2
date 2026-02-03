using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TrainingCourses.Queries
{
    public sealed class ListUserTrainingCoursesQuery : IRequest<IList<ListUserTrainingCoursesViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserTrainingCoursesQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserTrainingCoursesQuery, IList<ListUserTrainingCoursesViewModel>>
        {
            private readonly ITrainingCourseRepository trainingCourseRepository;
            private readonly IMapper mapper;

            public QueryHandler(ITrainingCourseRepository trainingCourseRepository, IMapper mapper)
            {
                this.trainingCourseRepository = trainingCourseRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserTrainingCoursesViewModel>> Handle(ListUserTrainingCoursesQuery request, CancellationToken cancellationToken)
            {
                var trainingCourses = await trainingCourseRepository.ListByUserIdAsync(request.Id, cancellationToken);
                return mapper.Map<IList<ListUserTrainingCoursesViewModel>>(trainingCourses);
            }
        }
        #endregion
    }
}
