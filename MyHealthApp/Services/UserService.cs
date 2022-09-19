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

        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        UserService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<User> PostUser(User user)
        {
            _requestUri = new Uri("http://192.168.1.15:8383/api/user");
            var json = JsonConvert.SerializeObject(user, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            
            return response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                : null;
        }
        
        public async Task<User> PostAuthenticateUser(string email,string password)
        {
            var myUser = new User(){Email = email,Password = password};
            _requestUri = new Uri("http://192.168.1.15:8383/api/user/authenticate");
            
            var json = JsonConvert.SerializeObject(myUser, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            //var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            var myNewUser = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                : null;
            
            return myNewUser;
            //return response.StatusCode == HttpStatusCode.OK ?  long.Parse((string)jObj["id"] ?? "-1") : -1;
        }
    }
}