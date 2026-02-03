using AutoMapper;
using Mbrcld.Application.Features.Scholarships.Queries;
using Mbrcld.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetScholarshipByIdQuery : IRequest<GetScholarshipByIdViewModel>
    {
        #region Query
        public Guid Id { get; }
        public Guid UserId { get; }

        public GetScholarshipByIdQuery(Guid id, Guid userid)
        {
            Id = id;
            UserId = userid;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetScholarshipByIdQuery, GetScholarshipByIdViewModel>
        {
            private readonly IScholarshipRepository scholarshipRepository;
            private readonly IScholarshipRegistrationRepository scholarshipRegistrationRepository;
            private readonly IMapper mapper;

            public QueryHandler(IScholarshipRepository scholarshipRepository, IScholarshipRegistrationRepository scholarshipRegistrationRepository, IMapper mapper)
            {
                this.scholarshipRepository = scholarshipRepository;
                this.scholarshipRegistrationRepository = scholarshipRegistrationRepository;
                this.mapper = mapper;
            }

            public async Task<GetScholarshipByIdViewModel> Handle(GetScholarshipByIdQuery request, CancellationToken cancellationToken)
            {
                var scholarships = await scholarshipRepository.GetScholarshipByIdAsync(request.Id).ConfigureAwait(false);
               if (scholarships.HasValue)
                {
                var scholarshipregistration = await scholarshipRegistrationRepository.GetScholarshipRegistrationByUserIdAndScholarshipIdAsync(request.UserId, request.Id).ConfigureAwait(false);
                if (scholarshipregistration.HasValue)
                {
                    scholarships.Value.AlreadyRegistered = true;
                }
                    return mapper.Map<GetScholarshipByIdViewModel>(scholarships.ValueOrDefault);
                }
                return null;
            }
        }
        #endregion
    }
}
