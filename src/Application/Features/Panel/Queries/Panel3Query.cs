using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Panel.Queries
{
    public sealed class Panel3Query : IRequest<IList<Panel3ViewModel>>
    {
        #region Query

        public Panel3Query()
        {
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<Panel3Query, IList<Panel3ViewModel>>
        {
            private readonly IEnrollmentRepository enrollmentRepository;
            private readonly IMapper mapper;

            public QueryHandler(IEnrollmentRepository enrollmentRepository, IMapper mapper)
            {
                this.enrollmentRepository = enrollmentRepository;
                this.mapper = mapper;
            }

            public async Task<IList<Panel3ViewModel>> Handle(Panel3Query request, CancellationToken cancellationToken)
            {
               // var result = await enrollmentRepository.GetILPEnrollments().ConfigureAwait(false);
                //var result2 = await enrollmentRepository.GetAllEnrollments().ConfigureAwait(false);
                var panel3 = new List<Panel3ViewModel>();
                panel3.Add(new Panel3ViewModel()
                {
                    Title = "MBRCLD Alumni",
                    Title_AR = "خريج",
                    Number = "700+"
                });
                panel3.Add(new Panel3ViewModel()
                {
                    Title = "Sectors",
                    Title_AR = "قطاع",
                    Number = "40+"
                });
                panel3.Add(new Panel3ViewModel()
                {
                    Title = "Transformational ideas and projects",
                    Title_AR = "فكرة ومشروع",
                    Number = "1000+"
                });
                return mapper.Map<List<Panel3ViewModel>>(panel3);
            }
        }
        #endregion
    }
}
