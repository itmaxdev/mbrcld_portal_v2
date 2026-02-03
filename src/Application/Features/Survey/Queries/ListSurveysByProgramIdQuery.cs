using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class ListSurveysByProgramIdQuery : IRequest<IList<ListSurveysByProgramIdViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListSurveysByProgramIdQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListSurveysByProgramIdQuery, IList<ListSurveysByProgramIdViewModel>>
        {
            private readonly ISurveyRepository surveyRepository;
            private readonly IConfiguration configuration;
            private readonly IMapper mapper;

            public QueryHandler(ISurveyRepository surveyRepository, IConfiguration configuration, IMapper mapper)
            {
                this.surveyRepository = surveyRepository;
                this.configuration = configuration;
                this.mapper = mapper;
            }

            public async Task<IList<ListSurveysByProgramIdViewModel>> Handle(ListSurveysByProgramIdQuery request, CancellationToken cancellationToken)
            {
                var surveys = await surveyRepository.ListSurveysByProgramIdAsync(request.UserId, cancellationToken);
                string surveyBaseUrl = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:SurveyBaseUrl").FirstOrDefault().Value;
                foreach (var survey in surveys)
                {
                    survey.SurveyURL = surveyBaseUrl + "/" + survey.Id;
                }
                return mapper.Map<IEnumerable<ListSurveysByProgramIdViewModel>>(surveys).ToList();
            }
        }
        #endregion
    }
}
