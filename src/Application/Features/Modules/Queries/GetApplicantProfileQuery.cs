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
    public sealed class GetApplicantProfileQuery : IRequest<GetApplicantProfileViewModel>
    {
        #region Query
        public Guid Id { get; }

        public GetApplicantProfileQuery(Guid id)
        {
            Id = id;
        }
        #endregion
        #region Query handler

        private sealed class QueryHandler : IRequestHandler<GetApplicantProfileQuery, GetApplicantProfileViewModel>
        {
            private readonly IModuleRepository moduleRepository;
            private readonly IProfessionalExperienceRepository professionalExperienceRepository;
            private readonly IEducationQualificationRepository educationQualificationRepository;
            private readonly IMapper mapper;

            public QueryHandler(IModuleRepository moduleRepository,IProfessionalExperienceRepository professionalExperienceRepository, IEducationQualificationRepository educationQualificationRepository, IMapper mapper)
            {
                this.moduleRepository = moduleRepository;
                this.professionalExperienceRepository = professionalExperienceRepository;
                this.educationQualificationRepository = educationQualificationRepository;
                this.mapper = mapper;
            }

            public async Task<GetApplicantProfileViewModel> Handle(GetApplicantProfileQuery request, CancellationToken cancellationToken)
            {
                var applicant = await moduleRepository.GetApplicantProfileAsync(request.Id, cancellationToken);
                var applicantModel = mapper.Map<GetApplicantProfileViewModel>(applicant);

                var proffessionalexperience = await professionalExperienceRepository.ApplicantLatestProfessionalExperienceAsync(request.Id, cancellationToken);
                if(proffessionalexperience != null)
                {
                    applicantModel.JobTitle = proffessionalexperience.JobTitle;
                    applicantModel.Organization = proffessionalexperience.OrganizationName;
                }

                var education = await educationQualificationRepository.ApplicantLatestEducationAsync(request.Id, cancellationToken);
                if (education != null)
                {
                    switch (education.Degree)
                    {
                        case 1:
                            applicantModel.Education = "Associate - " + education.University;
                            break;

                        case 2:
                            applicantModel.Education = "Bachelor - " + education.University;
                            break;

                        case 3:
                            applicantModel.Education = "Master - " + education.University;
                            break;

                        case 4:
                            applicantModel.Education = "Doctoral - " + education.University;
                            break;
                    }
                }

                return applicantModel;
            }
        }
        #endregion
    }
}
