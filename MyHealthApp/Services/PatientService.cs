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
            _requestUri = new Uri($"http://192.168.1.15:8383/api/profile/{patient.ProfileId.ToString()}/patient");
            var json = JsonConvert.SerializeObject(patient, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);
            var jObj = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            
            return response.StatusCode == HttpStatusCode.OK ?  JsonConvert.DeserializeObject<Patient>(response.Content.ReadAsStringAsync().Result)
                : null;
        }

        public async Task<Patient> GetPatientByProfileId(long profileId)
        {
            _requestUri = new Uri($"http://192.168.1.15:8383/api/profile/{profileId.ToString()}/patient");
            var response = await _client.GetAsync(_requestUri);
            
            var myPatient = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<Patient>(response.Content.ReadAsStringAsync().Result)
                : null;
            
            return myPatient;
        }
    }
}