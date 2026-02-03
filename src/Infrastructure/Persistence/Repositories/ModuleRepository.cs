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
    internal sealed class ModuleRepository : IModuleRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IUserRepository user;

        public ModuleRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IUserRepository user)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.user = user;
        }

        public async Task<Module> GetModuleByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odatamodule = await webApiClient.For<ODataModule>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Module>(odatamodule);
        }

        public async Task<IList<Module>> ListModulesByProgramIdAsync(Guid programId, Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            List<Module> modules = new List<Module>();
            IEnumerable<ODataModule> odataModules;

            if (User.Value.Role == 4) // Instructor
            {
                odataModules = await webApiClient.For<ODataModule>()
                    .Filter(p => p.Program.Id == programId)
                    .Filter(p => p.Instructor.ContactId == userId)
                    .OrderBy(o => o.Order)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else if (User.Value.Role == 6) // Admin
            {
                odataModules = await webApiClient.For<ODataModule>()
                    .Filter(p => p.Program.Id == programId)
                    .OrderBy(o => o.Order)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                var program = await webApiClient.For<ODataProgram>()
                .Key(programId)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);
                var activeCohortId = program.ActiveCohort.Id;
                odataModules = await webApiClient.For<ODataModule>()
                   .Filter(p => p.Program.Id == programId && p.Cohort.Id == activeCohortId)
                   .OrderBy(o => o.Order)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);
            }

            if (odataModules.Any())
            {
                if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
                {
                    foreach (var odataModule in odataModules)
                    {
                        Guid ModuleId = odataModule.Id;
                        var module = mapper.Map<Module>(odataModule);

                        var ModuleCompletion = await webApiClient.For<ODataModuleCompletion>()
                        .Filter(p => p.Module.Id == ModuleId)
                        .Filter(p => p.Contact.ContactId == userId)
                        .ProjectToModel()
                        .FindEntriesAsync(cancellationToken)
                        .ConfigureAwait(false);

                        if (ModuleCompletion.Any())
                        {
                            var ModuleCompletionPer = ModuleCompletion.FirstOrDefault().Completed;
                            module.Completed = Math.Round(ModuleCompletionPer);
                            module.InstructorId = odataModule.Instructor.ContactId;
                        }
                        else
                        {
                            module.Completed = 0;
                        }
                        modules.Add(module);
                    }
                    return this.mapper.Map<IList<Module>>(modules);
                }
                else
                {
                    foreach (var odataModule in odataModules)
                    {
                        var module = mapper.Map<Module>(odataModule);
                        module.Completed = null;
                        modules.Add(module);
                    }
                    return this.mapper.Map<IList<Module>>(modules);
                }
            }
            else
            {
                return this.mapper.Map<IList<Module>>(odataModules);
            }
        }

        public async Task<IList<Module>> ListModulesByEliteClubIdAsync(Guid eliteClubId, Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            List<Module> modules = new List<Module>();
            IEnumerable<ODataModule> odataModules;

            if (User.Value.Role == 4) // Instructor
            {
                odataModules = await webApiClient.For<ODataModule>()
                    .Filter(p => p.EliteClub.Id == eliteClubId)
                    .Filter(p => p.Instructor.ContactId == userId)
                    .OrderBy(o => o.Order)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                odataModules = await webApiClient.For<ODataModule>()
                   .Filter(p => p.EliteClub.Id == eliteClubId)
                   .OrderBy(o => o.Order)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);
            }

            if (odataModules.Any())
            {
                if (User.Value.Role == 3) //Alumni
                {
                    foreach (var odataModule in odataModules)
                    {
                        Guid ModuleId = odataModule.Id;
                        var module = mapper.Map<Module>(odataModule);

                        var ModuleCompletion = await webApiClient.For<ODataModuleCompletion>()
                        .Filter(p => p.Module.Id == ModuleId)
                        .Filter(p => p.Contact.ContactId == userId)
                        .ProjectToModel()
                        .FindEntriesAsync(cancellationToken)
                        .ConfigureAwait(false);

                        if (ModuleCompletion.Any())
                        {
                            var ModuleCompletionPer = ModuleCompletion.FirstOrDefault().Completed;
                            module.Completed = Math.Round(ModuleCompletionPer);
                            module.InstructorId = odataModule.Instructor.ContactId;
                        }
                        else
                        {
                            module.Completed = 0;
                        }
                        modules.Add(module);
                    }
                    return this.mapper.Map<IList<Module>>(modules);
                }
                else
                {
                    foreach (var odataModule in odataModules)
                    {
                        var module = mapper.Map<Module>(odataModule);
                        module.Completed = null;
                        modules.Add(module);
                    }
                    return this.mapper.Map<IList<Module>>(modules);
                }
            }
            else
            {
                return this.mapper.Map<IList<Module>>(odataModules);
            }
        }

        public async Task<IList<Module>> ListModulesByCohortIdAsync(Guid cohortId, CancellationToken cancellationToken = default)
        {
            var odataModules = await webApiClient.For<ODataModule>()
                .Filter(p => p.Cohort.Id == cohortId)
                .OrderBy(o => o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<IList<Module>>(odataModules);
        }

        public async Task UpdateAsync(Module module, CancellationToken cancellationToken = default)
        {
            var odataModule = this.mapper.Map<ODataModule>(module);

            await this.webApiClient.For<ODataModule>()
                .Key(odataModule)
                .Set(odataModule)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> ListModulesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var ModuleCompletions = await webApiClient.For<ODataModuleCompletion>()
            .Filter(p => p.Contact.ContactId == userId)
            .ProjectToModel()
            .FindEntriesAsync(cancellationToken)
            .ConfigureAwait(false);

            return ModuleCompletions.Count();
        }

        public async Task<IList<Module>> ListUserModulesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var ModuleCompletions = await webApiClient.For<ODataModuleCompletion>()
                    .Filter(p => p.Contact.ContactId == userId)
                    .ProjectToModel()
                    .FindEntriesAsync(cancellationToken)
                    .ConfigureAwait(false);

            List<Guid> ModulesIds = new List<Guid>();
            List<Module> modules = new List<Module>();
            Module module;

            foreach (var ModuleCompletion in ModuleCompletions)
            {
                if (ModulesIds.Count > 0)
                {
                    if (ModulesIds.Contains(ModuleCompletion.Module.Id) == false)
                    {
                        ModulesIds.Add(ModuleCompletion.Module.Id);
                        module = await GetModuleByIdAsync(ModuleCompletion.Module.Id);
                        module.Completed = Math.Round(ModuleCompletion.Completed);
                        modules.Add(module);
                    }
                }
                else
                {
                    ModulesIds.Add(ModuleCompletion.Module.Id);
                    module = await GetModuleByIdAsync(ModuleCompletion.Module.Id);
                    module.Completed = Math.Round(ModuleCompletion.Completed);
                    modules.Add(module);
                }
            }
            return this.mapper.Map<IList<Module>>(modules);
        }

        public async Task<User> GetApplicantProfileAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var applicant = await user.GetByIdAsync(id);
            return this.mapper.Map<User>(applicant);
        }

        public async Task<IList<User>> ListModuleApplicantsAsync(Guid moduleId, CancellationToken cancellationToken = default)
        {
            List<User> Applicants = new List<User>();
            var odataModulesCompletions = await webApiClient.For<ODataModuleCompletion>()
                .Filter(p => p.Module.Id == moduleId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var odataModulesCompletion in odataModulesCompletions)
            {
                var applicant = odataModulesCompletion.Contact;
                Applicants.Add(this.mapper.Map<User>(applicant));
            }
            return this.mapper.Map<IList<User>>(Applicants);
        }

        public async Task<University> GetUniversityProfileAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odatamodule = await webApiClient.For<ODataModule>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            if (odatamodule != null)
            {
                Guid instructorid = odatamodule.Instructor.ContactId;
                var instructor = await user.GetByIdAsync(instructorid);

                if (instructor.Value.UniversityId != null)
                {
                    var odataUniversity = await webApiClient.For<ODataUniversity>()
                    .Key(instructor.Value.UniversityId)
                    .ProjectToModel()
                    .FindEntryAsync(cancellationToken)
                    .ConfigureAwait(false);
                    return this.mapper.Map<University>(odataUniversity);
                }
            }
            return null;
        }

        public async Task<Module> GetCurrentModuleAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            List<Module> modules = new List<Module>();
            IEnumerable<ODataModule> odataModules;

            var cohortContact = await webApiClient.For<ODataCohortContact>()
                .Filter(p => p.Contact.ContactId == userId)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            if (cohortContact.Any())
            {
                var programId = cohortContact.First().Program.Id;
                var activeCohortId = cohortContact.First().Cohort.Id;

                odataModules = await webApiClient.For<ODataModule>()
                   .Filter(p => p.Program.Id == programId && p.Cohort.Id == activeCohortId && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
                   .OrderBy(o => o.Order)
                   .ProjectToModel()
                   .FindEntriesAsync(cancellationToken)
                   .ConfigureAwait(false);
                if (odataModules.Any())
                    return this.mapper.Map<Module>(odataModules.FirstOrDefault());
                else
                    return null;

            }
            else
            {
                return null;
            }
        }
    }
}
