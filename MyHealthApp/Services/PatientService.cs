using System;
using System.Collections;
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
    public class PatientService
    {
        //Patron Singleton
        private static Lazy<PatientService> _instance = new Lazy<PatientService>(()=> new PatientService());
        public static PatientService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;
        PatientService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<Patient> PostPatient(Patient patient)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/{patient.ProfileId.ToString()}/patient");
            var json = JsonConvert.SerializeObject(patient, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            try {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<Patient>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<Patient> GetPatientByProfileId(long profileId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/profile/{profileId.ToString()}/patient");
            

            try {
                var response = await _client.GetAsync(_requestUri);
                var myPatient = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<Patient>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myPatient;
            } 
            catch (HttpRequestException) 
            {
                return null;
            }
            
        }

        public async Task<IList<Patient>> GetAllPatients()
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient");
            

            try
            {
                var response = await _client.GetAsync(_requestUri);
                var myPatients = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<Patient>>(response.Content.ReadAsStringAsync().Result)
                : new List<Patient>();

                return myPatients;
            }
            catch (HttpRequestException)
            {
                return new List<Patient>();
            }
            
            
        }

        public async Task<Patient> PutPatientByPatientAndId(Patient patient, long patientId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}");
            var json = JsonConvert.SerializeObject(patient, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            try {
                var response = await _client.PutAsync(_requestUri, contentJson);

                var myPatient = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<Patient>(response.Content.ReadAsStringAsync().Result)
                    : null;

                return myPatient;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<HttpStatusCode> DeletePatient(long patientId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId}");

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