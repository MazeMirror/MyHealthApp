using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.Activities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyHealthApp.Services.Activities
{
    public class KilocalorieService
    {
        private static Lazy<KilocalorieService> _instance = new Lazy<KilocalorieService>(()=> new KilocalorieService());
        public static KilocalorieService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        KilocalorieService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<KilocalorieActivity> PostKilocalorieActivityByPatientId(long patientId, KilocalorieActivity kilocalorieActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-kilocalories/{patientId.ToString()}/kilocalorie");
            var json = JsonConvert.SerializeObject(kilocalorieActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<KilocalorieActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        
        public async Task<KilocalorieActivity> GetKilocalorieActivityByIdAndPatientIdAndDate(long id,long patientId, DateTime date)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-kilocalories/{patientId.ToString()}/kilocalorie/{id.ToString()}?date={date.ToString("d", CultureInfo.InvariantCulture)}");
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<KilocalorieActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        public async Task<List<KilocalorieActivity>> GetKilocalorieActivitiesByPatientIdAndDates(long patientId,DateTime startDate, DateTime endDate)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-kilocalories/{patientId.ToString()}/kilocalories?" +
                                  $"startDate={startDate.ToString("d", CultureInfo.InvariantCulture)}&" +
                                  $"endDate={endDate.ToString("d", CultureInfo.InvariantCulture)}");
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                var kilocalorieActivities = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<KilocalorieActivity>>(response.Content.ReadAsStringAsync().Result)
                    : new List<KilocalorieActivity>();

                return kilocalorieActivities;
            }
            catch (Exception)
            {
                return new List<KilocalorieActivity>();
            }
        }


        public async Task<KilocalorieActivity> UpdateKilocalorieActivityByPatientIdAndId(long patientId, long kilocalorieActivityId, KilocalorieActivity kilocalorieActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-kilocalories/{patientId.ToString()}/kilocalorie/{kilocalorieActivityId.ToString()}");
            var json = JsonConvert.SerializeObject(kilocalorieActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<KilocalorieActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}