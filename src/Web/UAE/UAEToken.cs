using Newtonsoft.Json;

namespace Mbrcld.Web.UAE
{
    public class UAEToken
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
    }
}
