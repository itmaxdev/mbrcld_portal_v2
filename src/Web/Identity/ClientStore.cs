using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mbrcld.Web.Identity
{
    public sealed class ClientStore : IClientStore
    {
        public static readonly List<Client> Clients = new List<Client>()
        {
            new Client()
            {
                ClientId = "dev",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AccessTokenType = AccessTokenType.Jwt,
                AccessTokenLifetime = 3600,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(14).TotalSeconds,
                UpdateAccessTokenClaimsOnRefresh = true,
                AllowedScopes = new[]
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                },
            }
        };

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return Clients.SingleOrDefault(c => c.ClientId == clientId);
        }
    }
}
