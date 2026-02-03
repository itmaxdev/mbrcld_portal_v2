using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Mbrcld.Web.UAE
{
    public interface IUAEService
    {
        Task<UAEProfile> GetProfile(string token);

        Task<UAEToken> GetToken(string code, string redirectUrl);        
    }

    public class UAEService : IUAEService
    {
        private readonly UAEOptions uaeOptions;

        public UAEService(IOptions<UAEOptions> options)
        {
            this.uaeOptions = options.Value;
        }


        public async Task<UAEProfile> GetProfile(string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var res = await client.GetAsync(uaeOptions.ProfileURL);

            if (!res.IsSuccessStatusCode) return null;

            var profileStr = await res.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<UAEProfile>(profileStr);

            return profile;
        }

        public async Task<UAEToken> GetToken(string code, string redirectUrl)
        {
            using var client = new HttpClient();
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("code", code),
            };

            var authenticationString = $"{uaeOptions.Username}:{uaeOptions.Password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            var content = new FormUrlEncodedContent(values);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uaeOptions.TokenURL);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = content;

            var res = await client.SendAsync(requestMessage);

            await res.Content.ReadAsStringAsync();
            
            if (!res.IsSuccessStatusCode) return null;

            var responseStr = await res.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<UAEToken>(responseStr);

            return response;
        }
    }
}
