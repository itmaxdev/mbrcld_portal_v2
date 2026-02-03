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
    internal sealed class ProgramRepository : IProgramRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IUserRepository user;

        public ProgramRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IUserRepository user)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.user = user;
        }

        public async Task<Program> GetProgramByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odataProgram = await webApiClient.For<ODataProgram>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Program>(odataProgram);
        }

        public async Task<Program> GetProgramDetailsByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            var odataProgram = await webApiClient.For<ODataProgram>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            //if (odataProgram.ActiveCohort != null)
            //{
            //    var activeCohort = odataProgram.ActiveCohort.Id;

            //    var odataEnrollment = await webApiClient.For<ODataEnrollment>()
            //    .Filter(p => p.Program.Id == id)
            //    .Filter(p => p.Contact.ContactId == userId)
            //    .Filter(p => p.Cohort.Id == activeCohort)
            //    .ProjectToModel()
            //    .FindEntryAsync(cancellationToken)
            //    .ConfigureAwait(false);
            //}

            return this.mapper.Map<Program>(odataProgram);
        }

        public async Task<Program> GetActiveProgramAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataPrograms = await webApiClient.For<ODataProgram>()
                .Filter(p => p.LastEnrollmentDate >= DateTime.Now)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (odataPrograms.Any())
            {
                Guid programId = odataPrograms.FirstOrDefault().Id;
                var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Program.Id == programId)
                .Filter(p => p.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

                if (!odataCohortContacts.Any())
                {
                    return this.mapper.Map<Program>(odataPrograms.FirstOrDefault());
                }
            }
            return null;
        }

        public async Task<Program> GetCurrentActiveProgramAsync(CancellationToken cancellationToken = default)
        {
            var odataPrograms = await webApiClient.For<ODataProgram>()
               .Filter(p => p.LastEnrollmentDate >= DateTime.Now)
               .OrderBy(p=>p.Order)
               .ProjectToModel()
               .FindEntriesAsync(cancellationToken)
               .ConfigureAwait(false);

            if (odataPrograms.Any())
            {
                return this.mapper.Map<Program>(odataPrograms.FirstOrDefault());
            }
            return null;
        }
    

        public async Task<IList<Program>> ListAlumniAvailableProgramAsync(Guid userId, string year, CancellationToken cancellationToken = default)
        {
            var period = int.Parse(year);
            var date = DateTime.Now.AddYears(-period);
            List<ODataProgram> programs = new List<ODataProgram>();
            bool canRegister = true;

            var odataPrograms = await webApiClient.For<ODataProgram>()
                .Filter(p => p.LastEnrollmentDate >= DateTime.Now)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var odataProgram in odataPrograms)
            {
                Guid programId = odataProgram.Id;
                var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Program.Id == programId)
                .Filter(p => p.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

                if (!odataCohortContacts.Any())
                {
                    programs.Add(odataProgram);
                }
            }

            var userCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var userCohortContact in userCohortContacts)
            {
                var enddate = userCohortContact.Cohort.EndDate;
                if (enddate > date)
                {
                    canRegister = false;
                }
            }

            if (canRegister == true)
            {
                return this.mapper.Map<IList<Program>>(programs);
            }
            return null;
        }

        public async Task<IList<Cohort>> ListAlumniGraduatedProgramAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            List<ODataCohort> cohorts = new List<ODataCohort>();

            var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
            .Filter(p => p.Contact.ContactId == userId)
            .Filter(p => p.Status == 936510000) //Graduated
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            foreach (var odataCohortContact in odataCohortContacts)
            {
                var odataCohorts = await webApiClient.For<ODataCohort>()
                .Key(odataCohortContact.Cohort.Id)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

                if (odataCohorts.Any())
                {
                    cohorts.Add(odataCohorts.FirstOrDefault());
                }
            }
            return this.mapper.Map<IList<Cohort>>(cohorts);
        }

        public async Task<IList<Program>> ListProgramsByUserModulesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);

            List<Guid> programIds = new List<Guid>();
            List<Program> programs = new List<Program>();
            IEnumerable<ODataModule> odataModules;

            if (User.Value.Role != 6) //Not Admin Role
            {
                odataModules = await webApiClient.For<ODataModule>()
                    .Filter(p => p.Instructor.ContactId == userId)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                odataModules = await webApiClient.For<ODataModule>()
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }


            foreach (var module in odataModules)
            {
                if (programIds.Count > 0)
                {
                    if (programIds.Contains(module.Program.Id) == false)
                    {
                        programIds.Add(module.Program.Id);
                    }
                }
                else
                {
                    programIds.Add(module.Program.Id);
                }
            }

            foreach (var programid in programIds)
            {
                programs.Add(await GetProgramByIdAsync(programid));
            }

            return this.mapper.Map<IList<Program>>(programs);
        }

        public async Task<IList<Program>> ListProgramsByCohortContactAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Contact.ContactId == userId)
                .Filter(p => p.Status == 1) //In Progress
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            List<Guid> programIds = new List<Guid>();
            List<Program> programs = new List<Program>();
            Program program;

            foreach (var CohortContact in odataCohortContacts)
            {
                if (programIds.Count > 0)
                {
                    if (programIds.Contains(CohortContact.Program.Id) == false)
                    {
                        programIds.Add(CohortContact.Program.Id);
                        program = await GetProgramByIdAsync(CohortContact.Program.Id);
                        program.Completed = Math.Round(CohortContact.Completed);
                        programs.Add(program);
                    }
                }
                else
                {
                    programIds.Add(CohortContact.Program.Id);
                    program = await GetProgramByIdAsync(CohortContact.Program.Id);
                    program.Completed = Math.Round(CohortContact.Completed);
                    programs.Add(program);
                }
            }

            return this.mapper.Map<IList<Program>>(programs);
        }

        public async Task<IList<Program>> ListUserProgramsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var odataCohortContacts = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            List<Guid> programIds = new List<Guid>();
            List<Program> programs = new List<Program>();
            Program program;

            foreach (var CohortContact in odataCohortContacts)
            {
                if (programIds.Count > 0)
                {
                    if (programIds.Contains(CohortContact.Program.Id) == false)
                    {
                        programIds.Add(CohortContact.Program.Id);
                        program = await GetProgramByIdAsync(CohortContact.Program.Id);
                        program.Completed = Math.Round(CohortContact.Completed);
                        programs.Add(program);
                    }
                }
                else
                {
                    programIds.Add(CohortContact.Program.Id);
                    program = await GetProgramByIdAsync(CohortContact.Program.Id);
                    program.Completed = Math.Round(CohortContact.Completed);
                    programs.Add(program);
                }
            }

            return this.mapper.Map<IList<Program>>(programs);
        }

        public async Task<IList<Program>> ListAllProgramsAsync(CancellationToken cancellationToken = default)
        {
            var odataPrograms = await webApiClient.For<ODataProgram>()
                .OrderBy(p => p.Name)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Program>>(odataPrograms);
        }

        public async Task<IList<Program>> ListActiveProgramsAsync(CancellationToken cancellationToken = default)
        {
            List<Program> programs = new List<Program>();

            var odataPrograms = await webApiClient.For<ODataProgram>()
                .Filter(p => p.Status == 1)
                .OrderBy(p => p.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            foreach (var odataProgram in odataPrograms)
            {
                var program = this.mapper.Map<Program>(odataProgram);
                if (odataProgram.LastEnrollmentDate >= DateTime.Now)
                {
                    program.OpenForRegistration = true;
                }
                programs.Add(program);
            }
            return programs;
        }

        public async Task<IList<int>> ListDistinctCohortYearsAsync(Guid programId,CancellationToken cancellationToken = default)
        {
            List<int> years = new List<int>();

            var odataCohorts = await webApiClient.For<ODataCohort>()
                .OrderBy(c => c.Year)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            odataCohorts = odataCohorts.Where(x => x.Program.Id == programId);
            foreach (var odataCohort in odataCohorts)
            {
                var year = odataCohort.Year;
                if (year.HasValue)
                {
                    if (!years.Contains(year.Value))
                    {
                        years.Add(year.Value);
                    }
                }
            }
            return years;
        }
    }
}