using AutoMapper;
using Mbrcld.Application.Interfaces;
using Mbrcld.Application.Interfaces.Repositories;
using Mbrcld.Domain.Entities;
using Mbrcld.Infrastructure.Extensions;
using Mbrcld.Infrastructure.Persistence.Models;
using Mbrcld.SharedKernel;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IPreferredLanguageService preferredLanguageService;

        public UserRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IPreferredLanguageService preferredLanguageService)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.preferredLanguageService = preferredLanguageService;
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            var odataContact = this.mapper.Map<ODataContact>(user);

            if (user.DirectManager == null)
            {
                odataContact.DirectManager = null;
            }

            if (user.UniversityId == null)
            {
                odataContact.University = null;
            }

            /**
             * This is a workaround for the multiselect optionset.
             * Upon creation, an empty value in the multiselect optionset value causes an error.
             * We are excluding do_learningpreferences and any other null values from the create request.
             */
            var dynamicObject = new Dictionary<string, object>();
            foreach (var property in odataContact.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = property.GetCustomAttribute<DataMemberAttribute>().Name;
                var value = property.GetValue(odataContact);

                if (value == null || (propertyName == "do_learningpreferences" && string.IsNullOrWhiteSpace(value as string)))
                {
                    continue;
                }

                if (this.preferredLanguageService.GetPreferredLanguageLCID() == 1025)
                {
                    if (propertyName == "firstname" || propertyName == "lastname")
                    {
                        dynamicObject.Add($"do_{propertyName}_ar", value);
                        continue;
                    }
                }

                dynamicObject.Add(propertyName, value);
            }

            await this.webApiClient.For<ODataContact>()
                .Set(dynamicObject)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var odataContact = this.mapper.Map<ODataContact>(user);

            if (user.DirectManager == null)
            {
                odataContact.DirectManager = null;
            }

            if (user.UniversityId == null)
            {
                odataContact.University = null;
            }

            /**
             * This is a workaround for related collections.
             * Exclude any property of type IList<> from the update request.
             */
            var dynamicObject = new Dictionary<string, object>();
            foreach (var property in odataContact.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    continue;
                }

                var propertyName = property.GetCustomAttribute<DataMemberAttribute>().Name;
                var value = property.GetValue(odataContact);

                dynamicObject.Add(propertyName, value);
            }

            await this.webApiClient.For<ODataContact>()
                .Key(dynamicObject)
                .Set(dynamicObject)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            await this.webApiClient.For<ODataContact>()
                .Key(user.Id)
                .DeleteEntryAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Maybe<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataUser = await this.webApiClient.For<ODataContact>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<User>(odataUser);
        }

        public async Task<Maybe<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var odataUser = await this.webApiClient.For<ODataContact>()
                .Filter(c => c.EmailAddress1 == email)
                .Select(c => c.ContactId)
                .FindEntryAsync()
                .ConfigureAwait(false);

            if (odataUser == null)
            {
                return null;
            }

            return await this.GetByIdAsync(odataUser.ContactId, cancellationToken);
        }

        public async Task<Maybe<User>> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            var phone = FormatPhone(phoneNumber);
            var odataUser = await this.webApiClient.For<ODataContact>()
                .Filter(c => c.MobilePhone == phone)
                .Select(c => c.ContactId)
                .FindEntryAsync()
                .ConfigureAwait(false);

            if (odataUser == null)
            {
                return null;
            }

            return await this.GetByIdAsync(odataUser.ContactId, cancellationToken);
        }

        public async Task<IList<User>> ListDirectManagerApplicantsAsync(Guid UserId, CancellationToken cancellationToken = default)
        {
            var odataApplicants = await this.webApiClient.For<ODataContact>()
                .Filter(c => c.DirectManager.ContactId == UserId)
                .Filter(c => c.Role == 2) //Applicant
                .ProjectToModel()
                .FindEntriesAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IList<User>>(odataApplicants);
        }
        public async Task<IList<User>> SearchAlumniAsync(string search, CancellationToken cancellationToken = default)
        {
            var odataUsers = await webApiClient.For<ODataContact>()
                .Filter(c => c.FirstName.Contains(search) || c.MiddleName.Contains(search) || c.LastName.Contains(search))
                .Filter(c => c.Role == 3) //Alumni
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<User>>(odataUsers);
        }
        public async Task<IList<User>> ListAlumniUsersForChatAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            List<Guid> CohortsList = new List<Guid> { };
            List<Guid> ContactIds = new List<Guid> { };
            List<User> Contacts = new List<User> { };

            var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(c => c.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var odataCohortContact in odataCohortContacts)
            {
                CohortsList.Add(odataCohortContact.Cohort.Id);
            }

            foreach (var Cohort in CohortsList)
            {
                var CohortContactList = await webApiClient.For<ODataCohortContact>()
                .Filter(c => c.Cohort.Id == Cohort)
                .Filter(c => c.Status == 936510000) // Graduated
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

                foreach (var CohortContact in CohortContactList)
                {
                    if (CohortContact.Contact.ContactId != userId)
                    {
                        ContactIds.Add(CohortContact.Contact.ContactId);
                    }
                }
            }

            foreach (var ContactId in ContactIds)
            {
                var contact = await GetByIdAsync(ContactId);
                if (contact.HasValue)
                {
                    Contacts.Add(contact.Value);
                }
            }

            return this.mapper.Map<IList<User>>(Contacts);
        }

        public async Task<IList<User>> SearchAlumniCriteriaAsync(Guid? programId, Guid? sectorId, int? year, Guid userId, CancellationToken cancellationToken = default)
        {
            List<string> ContactsByYear = new List<string> { };
            List<string> ContactsByProgram = new List<string> { };
            List<string> ContactsBySector = new List<string> { };

            List<User> Alumnies = new List<User> { };

            List<List<string>> lists = new List<List<string>>() { };

            if (year != null)
            {
                var odataCohorts = await webApiClient.For<ODataCohort>()
                .Filter(c => c.Year == year)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

                foreach (var odataCohort in odataCohorts)
                {
                    var odataCohortId = odataCohort.Id;
                    var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                   .Filter(c => c.Cohort.Id == odataCohortId)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);

                    foreach (var odataCohortContact in odataCohortContacts)
                    {
                        if (odataCohortContact.Contact.ContactId != userId)
                        {
                            if (!ContactsByYear.Contains(odataCohortContact.Contact.ContactId.ToString()))
                            {
                                ContactsByYear.Add(odataCohortContact.Contact.ContactId.ToString());
                            }
                        }
                    }
                }

                if (ContactsByYear.Count > 0)
                {
                    lists.Add(ContactsByYear);
                }
            }

            if (programId != null)
            {
                var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                   .Filter(c => c.Program.Id == programId)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);

                foreach (var odataCohortContact in odataCohortContacts)
                {
                    if (odataCohortContact.Contact.ContactId != userId)
                    {
                        if (!ContactsByProgram.Contains(odataCohortContact.Contact.ContactId.ToString()))
                        {
                            ContactsByProgram.Add(odataCohortContact.Contact.ContactId.ToString());
                        }
                    }
                }

                if (ContactsByProgram.Count > 0)
                {
                    lists.Add(ContactsByProgram);
                }
            }

            if (sectorId != null)
            {
                var odataProfessionalExperiences = await webApiClient.For<ODataProfessionalExperience>()
                   .Filter(c => c.Sector.SectorId == sectorId)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);

                foreach (var odataProfessionalExperience in odataProfessionalExperiences)
                {
                    if (odataProfessionalExperience.Contact.ContactId != userId)
                    {
                        if (!ContactsBySector.Contains(odataProfessionalExperience.Contact.ContactId.ToString()))
                        {
                            ContactsBySector.Add(odataProfessionalExperience.Contact.ContactId.ToString());
                        }
                    }
                }

                if (ContactsBySector.Count > 0)
                {
                    lists.Add(ContactsBySector);
                }
            }

            if (lists.Count > 0)
            {
                List<string> extList = lists.Cast<IEnumerable<string>>()
                            .Aggregate((a, b) => a.Intersect(b)).ToList();

                foreach (var id in extList)
                {
                    var user = await this.GetByIdAsync(Guid.Parse(id));
                    if (user.Value.Role == 3)//alumni
                    {
                        Alumnies.Add(user.Value);
                    }
                }
                return this.mapper.Map<IList<User>>(Alumnies);
            }

            return null;
        }

        private string FormatPhone(string phone)
        {
            decimal intPhone;
            if (Decimal.TryParse(phone, out intPhone))
            {
                if (phone.Length == 12)
                    phone = String.Format("{0:+### ## ### ####}", intPhone);
                if (phone.Length == 11)
                    phone = String.Format("{0:+### # ### ####}", intPhone);
            }

            return phone;
        }

        public async Task<User> GetByEmiratesIdAsync(string emiratesId, bool isUpdate = false, Guid? id = null, CancellationToken cancellationToken = default)
        {
            ODataContact odataUser;
            if (isUpdate)
            {
                odataUser = await this.webApiClient.For<ODataContact>()
                    .Filter(c => c.EmiratesId == emiratesId && c.ContactId != id)
                    .Select(c => c.ContactId)
                    .FindEntryAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                odataUser = await this.webApiClient.For<ODataContact>()
                        .Filter(c => c.EmiratesId == emiratesId)
                        .Select(c => c.ContactId)
                        .FindEntryAsync(cancellationToken)
                        .ConfigureAwait(false); 
            }

            return this.mapper.Map<User>(odataUser);
        }
    }
}
