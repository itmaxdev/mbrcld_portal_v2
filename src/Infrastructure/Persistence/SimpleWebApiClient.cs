using Simple.OData.Client;
using System.Net.Http;

namespace Mbrcld.Infrastructure.Persistence
{
    internal sealed class SimpleWebApiClient : ODataClient, ISimpleWebApiClient
    {
        public SimpleWebApiClient(HttpClient httpClient)
            : base(new ODataClientSettings(httpClient)
            {
                IgnoreResourceNotFoundException = true,
                //IgnoreUnmappedProperties = true,
            })
        {
        }
    }
}
