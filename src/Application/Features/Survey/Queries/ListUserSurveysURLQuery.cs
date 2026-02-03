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
    public sealed class ListUserSurveysURLQuery : IRequest<IList<ListUserSurveysUrlViewModel>>
    {
        #region Query
        public Guid UserId { get; }

        public ListUserSurveysURLQuery(Guid userid)
        {
            UserId = userid;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserSurveysURLQuery, IList<ListUserSurveysUrlViewModel>>
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

            public async Task<IList<ListUserSurveysUrlViewModel>> Handle(ListUserSurveysURLQuery request, CancellationToken cancellationToken)
            {
                var surveys = await surveyRepository.ListUserSurveysUrlAsync(request.UserId, cancellationToken);
                string surveyBaseUrl = configuration.GetSection("Mscrm").GetChildren().Where(x => x.Path == "Mscrm:SurveyBaseUrl").FirstOrDefault().Value;
                foreach (var survey in surveys)
                {
                    survey.SurveyURL = surveyBaseUrl + "/" + survey.Id;
                }
                return mapper.Map<IEnumerable<ListUserSurveysUrlViewModel>>(surveys).ToList();
            }
        }
        #endregion
    }
}
