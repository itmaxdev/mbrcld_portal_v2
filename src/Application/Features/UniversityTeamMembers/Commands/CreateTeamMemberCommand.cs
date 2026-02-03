using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Domain.ValueObjects;
using Mbrcld.SharedKernel.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Application.Features.TeamMember.Commands
{
    public sealed class CreateTeamMemberCommand : IRequest<Result>
    {
        #region Command
        public byte[] Content { get; }
        public string ContentType { get; }
        public string FileName { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AboutMember { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Education { get; set; }
        public string JobPosition { get; set; }
        public string ResidenceCountry { get; set; }
        public string LinkedIn { get; set; }
        public Guid UserId { get; set; }
        public CancellationToken CancellationToken { get; }

        public CreateTeamMemberCommand(
            byte[]? content,
            string? contentType,
            string? fileName,
            string firstname,
            string lastname,
            string aboutmember,
            string email,
            string nationality,
            string residencecountry,
            string jobposition,
            string education,
            string linkedin,
            Guid userId
            )
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.AboutMember = aboutmember;
            this.Email = email;
            this.ResidenceCountry = residencecountry;
            this.Nationality = nationality;
            this.JobPosition = jobposition;
            this.Education = education;
            this.LinkedIn = linkedin;
            this.UserId = userId;
        }
        #endregion

        #region Command handler
        public sealed class CommandHandler : IRequestHandler<CreateTeamMemberCommand, Result>
        {
            private readonly IUserProfilePictureService profilePictureService;
            private readonly IUniversityTeamMemberRepository teamMemberRepository;
            private readonly ICountryRepository countryRepository;
            private readonly IUserRepository userRepository;

            public CommandHandler(IUserProfilePictureService profilePictureService, IUniversityTeamMemberRepository teamMemberRepository,
                ICountryRepository countryRepository, IUserRepository userRepository)
            {
                this.profilePictureService = profilePictureService;
                this.teamMemberRepository = teamMemberRepository;
                this.countryRepository = countryRepository;
                this.userRepository = userRepository;
            }

            public async Task<Result> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
            {
                var nationalitycountry = await countryRepository.GetByIsoCodeAsync(request.Nationality);
                if (nationalitycountry.HasNoValue)
                {
                    return Result.Failure($"Invalid nationality for country code {request.Nationality}");
                }

                var residenceCountry = await countryRepository.GetByIsoCodeAsync(request.ResidenceCountry);
                if (residenceCountry.HasNoValue)
                {
                    return Result.Failure($"Invalid country code {request.ResidenceCountry}");
                }

                var user = await userRepository.GetByIdAsync(request.UserId);
                var university = user.Value.UniversityId;

                var teamMemberData = UniversityTeamMember.Create(
                        firstName: request.FirstName,
                        lastName: request.LastName,
                        fullname: request.FirstName + " " + request.LastName,
                        email: request.Email,
                        aboutmember: request.AboutMember,
                        education: request.Education,
                        nationality: nationalitycountry.Value,
                        residencecountry: residenceCountry.Value,
                        jobposition: request.JobPosition,
                        universityid: university,
                        linkedin: request.LinkedIn
                    );

                var returnedTeamMember = await teamMemberRepository.CreateAsync(teamMemberData).ConfigureAwait(false);
                var teamMemberId = returnedTeamMember.Value.Id;

                if (request.Content != null)
                {
                    return await this.profilePictureService.ChangeTeamMemberProfilePictureAsync(
                        teamMemberId,
                        request.Content,
                        cancellationToken
                        );
                }
                else
                {
                    return Result.Success(teamMemberId);
                }
            }
        }
        #endregion
    }
}
