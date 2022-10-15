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
    public class DistanceService
    {
        private static Lazy<DistanceService> _instance = new Lazy<DistanceService>(()=> new DistanceService());
        public static DistanceService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        DistanceService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<DistanceActivity> PostDistanceActivityByPatientId(long patientId, DistanceActivity distanceActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-distances/{patientId.ToString()}/distance");
            var json = JsonConvert.SerializeObject(distanceActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DistanceActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        
        public async Task<DistanceActivity> GetDistanceActivityByIdAndPatientIdAndDate(long id,long patientId, DateTime date)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-distances/{patientId.ToString()}/distance/{id.ToString()}?date={date.ToString("d", CultureInfo.InvariantCulture)}");
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DistanceActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        public async Task<List<DistanceActivity>> GetDistanceActivitiesByPatientIdAndDates(long patientId,DateTime startDate, DateTime endDate)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-distances/{patientId.ToString()}/distances?" +
                                  $"startDate={startDate.ToString("d", CultureInfo.InvariantCulture)}&" +
                                  $"endDate={endDate.ToString("d", CultureInfo.InvariantCulture)}");
            try
            {
                var response = await _client.GetAsync(_requestUri);

                var distanceActivities = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<DistanceActivity>>(response.Content.ReadAsStringAsync().Result)
                    : new List<DistanceActivity>();

                return distanceActivities;
            }
            catch (Exception)
            {
                return new List<DistanceActivity>();
            }
        }


        public async Task<DistanceActivity> UpdateDistanceActivityByPatientIdAndId(long patientId, long distanceActivityId, DistanceActivity distanceActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-distances/{patientId.ToString()}/distance/{distanceActivityId.ToString()}");
            var json = JsonConvert.SerializeObject(distanceActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DistanceActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}