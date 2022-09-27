﻿using System;
using System.Collections.Generic;
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
    public class SpecialistService
    {
        //Patron Singleton
        private static Lazy<SpecialistService> _instance = new Lazy<SpecialistService>(()=> new SpecialistService());
        public static SpecialistService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        SpecialistService()
        {
            
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<Specialist> PostSpecialist(Specialist specialist)
        {
            _requestUri = new Uri("https://myhealth-1664161814891.azurewebsites.net/api/specialist");
            var json = JsonConvert.SerializeObject(specialist, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            
            return response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<Specialist>(response.Content.ReadAsStringAsync().Result)
                : null;
        }

        public async Task<Specialist> GetSpecialistByProfileId(long profileId)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/profile/{profileId.ToString()}/specialist");
            var response = await _client.GetAsync(_requestUri);
            
            var mySpecialist = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<Specialist>(response.Content.ReadAsStringAsync().Result)
                : null;
            
            return mySpecialist;
        }
        
        public async Task<IList<Patient>> GetPatientsBySpecialistId(long specialistId)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/specialist/{specialistId.ToString()}/patients");
            var response = await _client.GetAsync(_requestUri);
            
            var myPatients = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<Patient>>(response.Content.ReadAsStringAsync().Result)
                : new List<Patient>();
            
            return myPatients;
        }

        public async Task<bool> AssignSpecialistWitPatient(long specialistId, long patientId)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/specialist/{specialistId.ToString()}/patients/{patientId.ToString()}");
            var contentJson = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_requestUri,contentJson);

            return response.StatusCode == HttpStatusCode.OK ? true : false;
        }

        public async Task<Specialist> PutSpecialistBySpecialistAndId(Specialist specialist, long specialistId)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/specialist/{specialistId.ToString()}");
            var json = JsonConvert.SerializeObject(specialist, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync(_requestUri,contentJson);
            
            var mySpecialist = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<Specialist>(response.Content.ReadAsStringAsync().Result)
                : null;
            
            return mySpecialist;
        }
    }
}