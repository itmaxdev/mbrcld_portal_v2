using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.Metadata.Queries
{
    public sealed class GetMentorByIdQuery : IRequest<GetMentorByIdViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetMentorByIdQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetMentorByIdQuery, GetMentorByIdViewModel>
        {
            private readonly IMentorRepository mentorRepository;
            private readonly IMapper mapper;

            public QueryHandler(IMentorRepository mentorRepository, IMapper mapper)
            {
                this.mentorRepository = mentorRepository;
                this.mapper = mapper;
            }

            public async Task<GetMentorByIdViewModel> Handle(GetMentorByIdQuery request, CancellationToken cancellationToken)
            {
                var mentor = await mentorRepository.GetMentorByIdAsync(request.Id).ConfigureAwait(false);
                return mapper.Map<GetMentorByIdViewModel>(mentor.ValueOrDefault);
            }
        }
        #endregion
    }
}
