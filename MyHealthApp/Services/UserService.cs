using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Globalization;

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
            _requestUri = new Uri("https://myhealthnewapi.azurewebsites.net/api/user");
            var json = JsonConvert.SerializeObject(user, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);


                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException e)
            {
                return null;
            }
            
        }
        
        public async Task<User> GetUserById(long userId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user/{userId.ToString()}");


            try
            {
                var response = await _client.GetAsync(_requestUri);


                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {

                return null;
            }
            
        }

        public async Task<User> GetUserByIdProfile(long userId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user/{userId.ToString()}/profile");


            try
            {
                var response = await _client.GetAsync(_requestUri);


                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {

                return null;
            }

        }

        public async Task<User> PostAuthenticateUser(string email,string password)
        {
            var myUser = new User(){Email = email,Password = password};
            _requestUri = new Uri("https://myhealthnewapi.azurewebsites.net/api/user/authenticate");
            
            var json = JsonConvert.SerializeObject(myUser, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");


            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);
                //var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                var myNewUser = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myNewUser;
            }
            catch (HttpRequestException)
            {

                return null;
            }
            
            //return response.StatusCode == HttpStatusCode.OK ?  long.Parse((string)jObj["id"] ?? "-1") : -1;
        }

        public async Task<User> PutUserEmail(User user)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user/{user.Id.ToString()}");
            
            var json = JsonConvert.SerializeObject(user, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);
                //var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                var myNewUser = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myNewUser;
            }
            catch (HttpRequestException)
            {

                return null;
            }
            
        }

        public async Task<HttpStatusCode> DeleteUserById(long userId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user/{userId.ToString()}");

            try
            {
                var response = await _client.DeleteAsync(_requestUri);

                return response.StatusCode;
            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.BadRequest;
            }

        }

        public async Task<List<User>> GetAllUsers()
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user");

            try
            {
                var response = await _client.GetAsync(_requestUri);
                var users = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<User>>(response.Content.ReadAsStringAsync().Result)
                : new List<User>();
                return users;
            }
            catch (HttpRequestException)
            {
                return new List<User>();
            }

        }
    }
}