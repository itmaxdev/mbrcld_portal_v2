using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Mbrcld.Infrastructure.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mbrcld.Infrastructure.Identity.Stores
{
    internal sealed class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly HttpClient httpClient;
        private readonly ISimpleWebApiClient webApiClient;
        private readonly IMapper mapper;

        public PersistedGrantStore(
            HttpClient httpClient,
            ISimpleWebApiClient webApiClient,
            IMapper mapper)
        {
            this.httpClient = httpClient;
            this.webApiClient = webApiClient;
            this.mapper = mapper;
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            var mscrmPersistedGrant = this.mapper.Map<PersistedGrantInMscrmModel>(grant);

            var key = mscrmPersistedGrant.Key = this.GetUrlSafeKey(mscrmPersistedGrant.Key);

            // This will perform an upsert request
            var content = new StringContent(
                JsonConvert.SerializeObject(
                    mscrmPersistedGrant,
                    new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    }),
                Encoding.UTF8,
                "application/json"
                );

            await this.httpClient.PatchAsync($"do_grants(do_key='{key}')", content)
                .ConfigureAwait(false);
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            key = this.GetUrlSafeKey(key);

            var grant = await this.webApiClient.For<PersistedGrantInMscrmModel>()
                .Filter(x => x.Key == key)
                .FindEntryAsync();

            return this.mapper.Map<PersistedGrant>(grant);
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var grant = await this.webApiClient.For<PersistedGrantInMscrmModel>()
                .Filter(x => x.ClientId == filter.ClientId)
                .Filter(x => x.SessionId == filter.SessionId)
                .Filter(x => x.SubjectId == filter.SubjectId)
                .Filter(x => x.Type == filter.Type)
                .FindEntriesAsync();

            return this.mapper.Map<IList<PersistedGrant>>(grant);
        }

        public async Task RemoveAsync(string key)
        {
            key = this.GetUrlSafeKey(key);

            await this.webApiClient.For<PersistedGrantInMscrmModel>()
                .Filter(x => x.Key == key)
                .DeleteEntriesAsync();
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            await this.webApiClient.For<PersistedGrantInMscrmModel>()
                .Filter(x => x.ClientId == filter.ClientId)
                .Filter(x => x.SessionId == filter.SessionId)
                .Filter(x => x.SubjectId == filter.SubjectId)
                .Filter(x => x.Type == filter.Type)
                .DeleteEntriesAsync();
        }

        private string GetUrlSafeKey(string originalKey)
        {
            if (originalKey == null) return null;
            return string.Join("", Encoding.ASCII.GetBytes(originalKey).Select(x => x.ToString("X2")).Take(50));
        }

        [DataContract(Name = "do_grant")]
        private sealed class PersistedGrantInMscrmModel
        {
            [DataMember(Name = "do_key")]
            public string Key { get; set; }

            [DataMember(Name = "do_type")]
            public string Type { get; set; }

            [DataMember(Name = "do_subjectid")]
            public string SubjectId { get; set; }

            [DataMember(Name = "do_sessionid")]
            public string SessionId { get; set; }

            [DataMember(Name = "do_clientid")]
            public string ClientId { get; set; }

            [DataMember(Name = "do_description")]
            public string Description { get; set; }

            [DataMember(Name = "do_creationtime")]
            public DateTime CreationTime { get; set; }

            [DataMember(Name = "do_expiration")]
            public DateTime? Expiration { get; set; }

            [DataMember(Name = "do_consumedtime")]
            public DateTime? ConsumedTime { get; set; }

            [DataMember(Name = "do_data")]
            public string Data { get; set; }
        }

        private sealed class PersistedGrantMappingProfile : Profile
        {
            public PersistedGrantMappingProfile()
            {
                CreateMap<PersistedGrant, PersistedGrantInMscrmModel>()
                    .ForMember(dst => dst.Key, x => x.MapFrom(src => src.Key))
                    .ForMember(dst => dst.Type, x => x.MapFrom(src => src.Type))
                    .ForMember(dst => dst.SubjectId, x => x.MapFrom(src => src.SubjectId))
                    .ForMember(dst => dst.SessionId, x => x.MapFrom(src => src.SessionId))
                    .ForMember(dst => dst.ClientId, x => x.MapFrom(src => src.ClientId))
                    .ForMember(dst => dst.Description, x => x.MapFrom(src => src.Description))
                    .ForMember(dst => dst.CreationTime, x => x.MapFrom(src => src.CreationTime))
                    .ForMember(dst => dst.Expiration, x => x.MapFrom(src => src.Expiration))
                    .ForMember(dst => dst.ConsumedTime, x => x.MapFrom(src => src.ConsumedTime))
                    .ForMember(dst => dst.Data, x => x.MapFrom(src => src.Data))
                    .ReverseMap();
            }
        }
    }
}
