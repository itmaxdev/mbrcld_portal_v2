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
    internal sealed class MaterialRepository : IMaterialRepository
    {
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;
        private readonly IUserRepository user;

        public MaterialRepository(ISimpleWebApiClient webApiClient, IMapper mapper, IUserRepository user)
        {
            this.webApiClient = webApiClient;
            this.mapper = mapper;
            this.user = user;
        }

        public async Task<Material> GetMaterialByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var odatamaterial = await webApiClient.For<ODataMaterial>()
                .Key(id)
                .ProjectToModel()
                .FindEntryAsync(cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Material>(odatamaterial);
        }

        public async Task<IList<Material>> ListMaterialsByModuleIdAsync(Guid moduleid, Guid userId, CancellationToken cancellationToken = default)
        {
            var User = await user.GetByIdAsync(userId);
            List<Material> materials = new List<Material>();
            IEnumerable<ODataMaterial> odataMaterials;

            if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
            {
               odataMaterials = await webApiClient.For<ODataMaterial>()
              .Filter(p => p.Module.Id == moduleid)
              .Filter(p => p.Status == 1)//Published
              .OrderBy(o => o.Order)
              .ProjectToModel()
              .FindEntriesAsync(cancellationToken)
              .ConfigureAwait(false);
            }
            else
            {
                odataMaterials = await webApiClient.For<ODataMaterial>()
               .Filter(p => p.Module.Id == moduleid)
               .OrderBy(o => o.Order)
               .ProjectToModel()
               .FindEntriesAsync(cancellationToken)
               .ConfigureAwait(false);
            }

            if (odataMaterials != null)
            {
                if (User.Value.Role == 2 || User.Value.Role == 3) //Applicant or Alumni
                {
                    foreach (var odataMaterial in odataMaterials)
                    {
                        Guid MaterialId = odataMaterial.Id;
                        var material = mapper.Map<Material>(odataMaterial);

                        var MaterialCompletion = await webApiClient.For<ODataMaterialCompletion>()
                        .Filter(p => p.Material.Id == MaterialId)
                        .Filter(p => p.Contact.ContactId == userId)
                        .ProjectToModel()
                        .FindEntriesAsync(cancellationToken)
                        .ConfigureAwait(false);

                        if (MaterialCompletion.Any())
                        {
                            var MaterialCompletionPer = MaterialCompletion.FirstOrDefault().Completed;
                            material.Completed = Math.Round(MaterialCompletionPer);
                        }
                        else
                        {
                            material.Completed = 0;
                        }
                        materials.Add(material);
                    }
                    return this.mapper.Map<IList<Material>>(materials);
                }
                else
                {
                    foreach (var odataMaterial in odataMaterials)
                    {
                        var material = mapper.Map<Material>(odataMaterial);
                        material.Completed = null;
                        materials.Add(material);
                    }
                    return this.mapper.Map<IList<Material>>(materials);
                }
            }
            else
            {
                return this.mapper.Map<IList<Material>>(odataMaterials);
            }
        }

        public async Task<IList<Material>> ListMaterialsByCohortModuleIdAsync(Guid moduleid, CancellationToken cancellationToken = default)
        {
            var odataMaterials = await webApiClient.For<ODataMaterial>()
                .Filter(p => p.Module.Id == moduleid)
                .Filter(p => p.Status == 1)//Published
                .OrderBy(o => o.Order)
                .ProjectToModel()
                .FindEntriesAsync(cancellationToken)
                .ConfigureAwait(false);
            return this.mapper.Map<IList<Material>>(odataMaterials);
        }

        public async Task<Maybe<Material>> CreateAsync(Material material, CancellationToken cancellationToken = default)
        {
            var odataMaterial = this.mapper.Map<ODataMaterial>(material);
            //odataArticle.CreatedOn = DateTime.UtcNow;

            await webApiClient.For<ODataMaterial>()
                .Set(odataMaterial)
                .InsertEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);

            return this.mapper.Map<Material>(odataMaterial);
        }

        public async Task UpdateAsync(Material material, CancellationToken cancellationToken = default)
        {
            var odataMaterial = this.mapper.Map<ODataMaterial>(material);

            await webApiClient.For<ODataMaterial>()
                .Key(odataMaterial)
                .Set(odataMaterial)
                .UpdateEntryAsync(false, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}