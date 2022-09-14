using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MyHealthApp.Services
{
    public class ProfileService
    {
        //Patron Singleton
        private static Lazy<ProfileService> _instance = new Lazy<ProfileService>(()=> new ProfileService());
        public static ProfileService Instance => _instance.Value;
        
        private readonly Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        ProfileService()
        {
            _requestUri = new Uri("http://192.168.1.15:8383/api/profile");
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<long> PostProfile(Profile profile)
        {
            var json = JsonConvert.SerializeObject(profile, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            
            return response.StatusCode == HttpStatusCode.OK ?  long.Parse((string)jObj["id"] ?? "-1") : -1;
        }
    }
}