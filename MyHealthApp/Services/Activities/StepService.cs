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
    public class StepService
    {
        private static Lazy<StepService> _instance = new Lazy<StepService>(()=> new StepService());
        public static StepService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        StepService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<StepActivity> PostStepActivityByPatientId(long patientId, StepActivity stepActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-steps/{patientId.ToString()}/step");
            var json = JsonConvert.SerializeObject(stepActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<StepActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        
        public async Task<StepActivity> GetStepActivityByIdAndPatientIdAndDate(long id,long patientId, DateTime date)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-steps/{patientId.ToString()}/step/{id.ToString()}?date={date.ToString("d", CultureInfo.InvariantCulture)}");
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<StepActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        
        public async Task<List<StepActivity>> GetStepActivitiesByPatientIdAndDates(long patientId,DateTime startDate, DateTime endDate)
        {
            if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            {
                DateTime date = DateTime.Today;
                int day = (int) date.DayOfWeek;
                DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
                DateTime sunday = date.AddDays((1) * (day== 0 ? day : 7 - day));
                
                _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-steps/{patientId.ToString()}/steps?" +
                                      $"startDate={monday.ToString("d", CultureInfo.InvariantCulture)}&" +
                                      $"endDate={sunday.ToString("d", CultureInfo.InvariantCulture)}");
            }
            else
            {
                _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-steps/{patientId.ToString()}/steps?" +
                                      $"startDate={startDate.ToString("d", CultureInfo.InvariantCulture)}&" +
                                      $"endDate={endDate.ToString("d", CultureInfo.InvariantCulture)}");
            }
            
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                var stepActivities = response.StatusCode == HttpStatusCode.OK
                    ? JsonConvert.DeserializeObject<List<StepActivity>>(response.Content.ReadAsStringAsync().Result)
                    : new List<StepActivity>();

                return stepActivities;
            }
            catch (Exception)
            {
                return new List<StepActivity>();
            }
        }


        public async Task<StepActivity> UpdateStepActivityByPatientIdAndId(long patientId, long stepActivityId, StepActivity stepActivity)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient-steps/{patientId.ToString()}/step/{stepActivityId.ToString()}");
            var json = JsonConvert.SerializeObject(stepActivity, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<StepActivity>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}