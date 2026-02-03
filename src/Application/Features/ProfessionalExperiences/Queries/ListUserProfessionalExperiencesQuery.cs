using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.ProfessionalExperiences.Queries
{
    public sealed class ListUserProfessionalExperiencesQuery : IRequest<IList<ListUserProfessionalExperiencesViewModel>>
    {
        #region Query
        public Guid Id { get; }
        public bool Completed { get; set; }

        public ListUserProfessionalExperiencesQuery(Guid id)
        {
            Id = id;
            //Completed = Completed;
         }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserProfessionalExperiencesQuery, IList<ListUserProfessionalExperiencesViewModel>>
        {
            private readonly IProfessionalExperienceRepository professionalExperienceRepository;
            private readonly IMapper mapper;

            public QueryHandler(IProfessionalExperienceRepository professionalExperienceRepository, IMapper mapper)
            {
                this.professionalExperienceRepository = professionalExperienceRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserProfessionalExperiencesViewModel>> Handle(ListUserProfessionalExperiencesQuery request, CancellationToken cancellationToken)
            {
                var professionalExperiences = await professionalExperienceRepository.ListByUserIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<IList<ListUserProfessionalExperiencesViewModel>>(professionalExperiences);
                
            }
        }
        #endregion
    }
}
