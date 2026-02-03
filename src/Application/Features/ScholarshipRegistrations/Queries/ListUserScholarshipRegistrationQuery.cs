using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Mbrcld.Domain.Entities;

namespace Mbrcld.Application.Features.ScholarshipRegistrations.Queries
{
    public sealed class ListUserScholarshipRegistrationQuery : IRequest<IList<ListUserScholarshipRegistrationViewModel>>
    {
        #region Query
        public Guid Id { get; }

        public ListUserScholarshipRegistrationQuery(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Query handler
        private sealed class QueryHandler : IRequestHandler<ListUserScholarshipRegistrationQuery, IList<ListUserScholarshipRegistrationViewModel>>
        {
            private readonly IScholarshipRegistrationRepository scholarshipRegistrationRepository;
            private readonly IMapper mapper;

            public QueryHandler(IScholarshipRegistrationRepository scholarshipRegistrationRepository, IMapper mapper)
            {
                this.scholarshipRegistrationRepository = scholarshipRegistrationRepository;
                this.mapper = mapper;
            }

            public async Task<IList<ListUserScholarshipRegistrationViewModel>> Handle(ListUserScholarshipRegistrationQuery request, CancellationToken cancellationToken)
            {
                var scholarshipRegistration = await scholarshipRegistrationRepository.ScholarshipRegistrationByUserIdAsync(request.Id).ConfigureAwait(false);
                var scholarshipRegistrationViewModels = scholarshipRegistration.Select(scholarshipRegistration => new ListUserScholarshipRegistrationViewModel
                {
                    ScholarshipId = scholarshipRegistration.ScholarshipId,
                    Registrant = scholarshipRegistration.UserId,
                    Name = scholarshipRegistration.Name,
                    StatusCode = Enum.GetName(typeof(ScholarshipRegistrationStatus), scholarshipRegistration.StatusCode)
                }).ToList();
                return scholarshipRegistrationViewModels;
            }
        }
        #endregion
    }
}