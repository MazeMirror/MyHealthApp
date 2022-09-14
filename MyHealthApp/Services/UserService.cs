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
    public class UserService
    {
        //Patron Singleton
        private static Lazy<UserService> _instance = new Lazy<UserService>(()=> new UserService());
        public static UserService Instance => _instance.Value;

        private readonly Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        UserService()
        {
            _requestUri = new Uri("http://192.168.1.15:8383/api/user");
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<long> PostUser(User user)
        {
            var json = JsonConvert.SerializeObject(user, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            
            return response.StatusCode == HttpStatusCode.OK ?  long.Parse((string)jObj["id"] ?? "-1") : -1;
        }
    }
}