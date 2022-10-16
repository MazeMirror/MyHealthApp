﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
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
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        ProfileService()
        {
            
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<Profile> PostProfile(Profile profile)
        {
            _requestUri = new Uri("https://myhealthnewapi.azurewebsites.net/api/profile");
            var json = JsonConvert.SerializeObject(profile, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            try {
                var response = await _client.PostAsync(_requestUri, contentJson);


                return response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<Profile>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<Profile> GetProfileByUserId(long userId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/user/{userId.ToString()}/profile");
           
            try
            {
                var response = await _client.GetAsync(_requestUri);
                var myProfile = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<Profile>(response.Content.ReadAsStringAsync().Result)
                : null;

                return myProfile;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<Profile> GetProfileById(long id)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/{id.ToString()}");

            try
            {
                var response = await _client.GetAsync(_requestUri);

                var myProfile = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<Profile>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myProfile;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
            
        }

        public async Task<Profile> PutProfileByProfileAndId(Profile profile, long id)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/{id.ToString()}");
            var json = JsonConvert.SerializeObject(profile, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");


            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                var myProfile = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<Profile>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myProfile;

            } catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<IList<Profile>> GetProfileByNameAndRoleId(string searchBarText, string roleId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile?name={searchBarText}&roleId={roleId}");

            try {
                var response = await _client.GetAsync(_requestUri);

                var myProfile = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<Profile>>(response.Content.ReadAsStringAsync().Result)
                    : new List<Profile>();

                return myProfile;
            }
            catch (HttpRequestException)
            {
                return new List<Profile>();
            }
            
        }

        public async Task<IList<Profile>> GetProfilesByRoleId(int roleId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/?roleId={roleId}");

            try
            {
                var response = await _client.GetAsync(_requestUri);

                var myProfile = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<Profile>>(response.Content.ReadAsStringAsync().Result)
                    : new List<Profile>();

                return myProfile;
            }
            catch (HttpRequestException)
            {

                return new List<Profile>();
            }
            
        }

        public async Task<HttpStatusCode> DeleteProfileById(long profileId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/{profileId}");

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
    }
}