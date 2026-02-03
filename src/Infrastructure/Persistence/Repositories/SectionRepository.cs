using AutoMapper;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Mbrcld.SharedKernel.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class SectionRepository : ISectionRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IUserRepository user;

        public SectionRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IUserRepository user)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.user = user;
        }

        public async Task<Section> GetSectionByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var odatasection = await webApiClient.For<ODataSection>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            if (odatasection != null)
            {
                var User = await user.GetByIdAsync(userId);
                if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
                {
                    Guid SectionId = odatasection.Id;
                    var section = mapper.Map<Section>(odatasection);

                    var SectionCompletion = await webApiClient.For<ODataSectionCompletion>()
                    .Filter(p => p.Section.Id == SectionId)
                    .Filter(p => p.Contact.ContactId == userId)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);

                    if (SectionCompletion.Any())
                    {
                        var SectionCompletionStatus = SectionCompletion.FirstOrDefault().Status;
                        section.Status = SectionCompletionStatus;
                    }
                    else
                    {
                        section.Status = 1;
                    }
                    return this.mapper.Map<Section>(section);
                }
                else
                {
                    var section = mapper.Map<Section>(odatasection);
                    section.Status = null;
                    return this.mapper.Map<Section>(section);
                }
            }
            else
            {
                return this.mapper.Map<Section>(odatasection);
            }
        }

        public async Task<IList<Section>> ListSectionsByMaterialIdAsync(Guid materialId, Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            List<Section> sections = new List<Section>();
            IEnumerable<ODataSection> odatasections;

            if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
            {
                odatasections = await webApiClient.For<ODataSection>()
                .Filter(p => p.Material.Id == materialId)
                .Filter(p => p.SectionStatus == 1)//Published
                .OrderBy(o => o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            }
            else
            {
                odatasections = await webApiClient.For<ODataSection>()
                .Filter(p => p.Material.Id == materialId)
                .OrderBy(o => o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            }

            if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
            {
                foreach (var odatasection in odatasections)
                {
                    Guid SectionId = odatasection.Id;
                    var section = mapper.Map<Section>(odatasection);

                    var SectionCompletion = await webApiClient.For<ODataSectionCompletion>()
                    .Filter(p => p.Section.Id == SectionId)
                    .Filter(p => p.Contact.ContactId == userId)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);

                    if (SectionCompletion.Any())
                    {
                        var SectionCompletionStatus = SectionCompletion.FirstOrDefault().Status;
                        section.Status = SectionCompletionStatus;
                    }
                    else
                    {
                        section.Status = 1;
                    }
                    sections.Add(section);
                }
                return this.mapper.Map<IList<Section>>(sections);
            }
            else
            {
                foreach (var odatasection in odatasections)
                {
                    var section = mapper.Map<Section>(odatasection);
                    section.Status = null;
                    sections.Add(section);
                }
                return this.mapper.Map<IList<Section>>(sections);
            }
        }

        public async Task<IList<Section>> ListSectionsByCohortMaterialIdAsync(Guid materialId, CancellationToken cancellationToken = default)
        {
            var odatasections = await webApiClient.For<ODataSection>()
                .Filter(p => p.Material.Id == materialId)
                .Filter(p => p.SectionStatus == 1)//Published
                .OrderBy(o => o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Section>>(odatasections);
        }

        public async Task<Maybe<Section>> CreateAsync(Section section, CancellationToken cancellationToken = default)
        {
            var odatasection = this.mapper.Map<ODataSection>(section);
            //odataArticle.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataSection>()
                 .Set(odatasection)
                 .InsertEntryAsync(false, cancellationToken)
                 .ConfigureAwait(false);

            return this.mapper.Map<Section>(odatasection);
        }

        public async Task UpdateAsync(Section section, CancellationToken cancellationToken = default)
        {
            var odatasection = this.mapper.Map<ODataSection>(section);

            await webApiClient.For<ODataSection>()
                .Key(odatasection)
                .Set(odatasection)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateSectionStatusAsync(Guid sectionId, Guid userId, int status, CancellationToken cancellationToken = default)
        {
            var odatasectionCompletions = await webApiClient.For<ODataSectionCompletion>()
                .Filter(p => p.Contact.ContactId == userId)
                .Filter(p => p.Section.Id == sectionId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (odatasectionCompletions.Any())
            {
                var odatasectioncompletion = this.mapper.Map<ODataSectionCompletion>(odatasectionCompletions.FirstOrDefault());
                odatasectioncompletion.Status = status;

                await webApiClient.For<ODataSectionCompletion>()
                    .Key(odatasectioncompletion)
                    .Set(odatasectioncompletion)
                    .UpdateEntryAsync(false, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<int> ListUnreadSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var SectionCompletions = await webApiClient.For<ODataSectionCompletion>()
            .Filter(p => p.Contact.ContactId == userId)
            .Filter(p => p.Status == 1) //Unread
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);
            return SectionCompletions.Count();
        }

        public async Task<int> ListReviewSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var SectionCompletions = await webApiClient.For<ODataSectionCompletion>()
            .Filter(p => p.Contact.ContactId == userId)
            .Filter(p => p.Status == 936510000) //Review
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);
            return SectionCompletions.Count();
        }

        public async Task<int> ListDoneSectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var SectionCompletions = await webApiClient.For<ODataSectionCompletion>()
            .Filter(p => p.Contact.ContactId == userId)
            .Filter(p => p.Status == 936510001) //Done
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);
            return SectionCompletions.Count();
        }
    }
}