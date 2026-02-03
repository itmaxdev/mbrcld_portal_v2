using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mbrcld.Web.UAE
{
    public class UAEProfile
    {
        [JsonProperty("idType")]
        public string IdType { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("lastnameEN")]
        public string LastnameEN { get; set; }

        [JsonProperty("nationalityAR")]
        public string NationalityAR { get; set; }

        [JsonProperty("firstnameEN")]
        public string FirstnameEN { get; set; }

        [JsonProperty("idn")]
        public string Idn { get; set; }

        [JsonProperty("userType")]
        public string UserType { get; set; }

        [JsonProperty("fullnameAR")]
        public string FullnameAR { get; set; }

        [JsonProperty("fullnameEN")]
        public string FullnameEN { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("nationalityEN")]
        public string NationalityEN { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("firstnameAR")]
        public string FirstnameAR { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("lastnameAR")]
        public string LastnameAR { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("acr")]
        public string Acr { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("amr")]
        public List<string> Amr { get; set; }
    }
}
