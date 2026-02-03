using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.LanguageSkills.Queries
{
    public sealed class ListUserLanguageSkillsQuery : IRequest<IList<ListUserLanguageSkillsViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserLanguageSkillsQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserLanguageSkillsQuery, IList<ListUserLanguageSkillsViewModel>>
        {
            private readonly ILanguageSkillRepository languageSkillRepository;
            private readonly IMapper mapper;

            public QueryHandler(ILanguageSkillRepository languageSkillRepository, IMapper mapper)
            {
                this.languageSkillRepository = languageSkillRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserLanguageSkillsViewModel>> Handle(ListUserLanguageSkillsQuery request, CancellationToken cancellationToken)
            {
                var LanguageSkills = await languageSkillRepository.ListByUserIdAsync(request.Id, cancellationToken);
                return mapper.Map<IList<ListUserLanguageSkillsViewModel>>(LanguageSkills);
            }
        }
        #endregion
    }
}
