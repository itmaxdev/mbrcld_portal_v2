using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Mbrcld.Web.Identity
{
    public sealed class ResourceStore : IResourceStore
    {
        public static readonly IdentityResource[] identityResources = new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return identityResources.Where(r => scopeNames.Contains(r.Name));
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return Array.Empty<ApiScope>();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Array.Empty<ApiResource>();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return Array.Empty<ApiResource>();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            return new Resources(
                Array.Empty<IdentityResource>(),
                Array.Empty<ApiResource>(),
                Array.Empty<ApiScope>()
                );
        }
    }
}
