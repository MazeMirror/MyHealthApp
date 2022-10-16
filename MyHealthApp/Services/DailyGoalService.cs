using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xamarin.Essentials;

namespace MyHealthApp.Services
{
    public class DailyGoalService
    {
        private static Lazy<DailyGoalService> _instance = new Lazy<DailyGoalService>(()=> new DailyGoalService());
        public static DailyGoalService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        DailyGoalService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        /*public async Task<List<DailyGoal>> GetDailyGoalsByPatientId(long patientId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoals");
            var response = await _client.GetAsync(_requestUri);
            
            var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();
            
            return dailyGoals;
        }*/

        public async Task<DailyGoal> PostDailyGoalByPatientId(long patientId, DailyGoal dailyGoal)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal");
            var json = JsonConvert.SerializeObject(dailyGoal, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.Created ? JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
            
        }

        public async Task<DailyGoal> PutDailyGoalByPatientId(long patientId, DailyGoal daily)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal/{daily.Id.ToString()}");
            var json = JsonConvert.SerializeObject(daily, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }


        /// <summary>
        /// Actulizacion por ActivityId
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="daily"></param>
        /// <returns></returns>
        public async Task<DailyGoal> PutDailyGoalStepByPatientId(long patientId, DailyGoal daily)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal/{daily.Id.ToString()}/step");
            var json = JsonConvert.SerializeObject(daily, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }

            
        }

        public async Task<DailyGoal> PutDailyGoalDistanceByPatientId(long patientId, DailyGoal daily)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal/{daily.Id.ToString()}/distance");
            var json = JsonConvert.SerializeObject(daily, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<DailyGoal> PutDailyGoalKilocalorieByPatientId(long patientId, DailyGoal daily)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal/{daily.Id.ToString()}/kilocalorie");
            var json = JsonConvert.SerializeObject(daily, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch(HttpRequestException)
            {
                return null;
            }
            
        }









        public async Task<HttpStatusCode> DeleteDailyGoalByPatientId(long patientId, DailyGoal daily)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoalId/{daily.Id.ToString()}");

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

        public async Task<List<DailyGoal>> GetDailyGoalsByPatientIdAndDate(long patientId, DateTime now)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoals?date={now.ToString("d", CultureInfo.InvariantCulture)}");
           

            try{
                var response = await _client.GetAsync(_requestUri);
                var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();

                return dailyGoals;
            }
            catch (Exception)
            {
                return new List<DailyGoal>();
            }
            
        }

        public async Task<List<DailyGoal>> GetDailyGoalsByPatientIdAndDates(long patientId,DateTime startDate, DateTime endDate)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoals/filter?" +
                                  $"startDate={startDate.ToString("d", CultureInfo.InvariantCulture)}&" +
                                  $"endDate={endDate.ToString("d", CultureInfo.InvariantCulture)}");
            
            
            try
            {
                var response = await _client.GetAsync(_requestUri);

                var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();

                return dailyGoals;
            }
            catch (Exception)
            {
                return new List<DailyGoal>();
            }
        }

        public async Task<List<DailyGoal>> GetAllDailyGoals()
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/dailyGoal");

            try
            {
                var response = await _client.GetAsync(_requestUri);
                var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();

                return dailyGoals;
            }
            catch (Exception)
            {
                return new List<DailyGoal>();
            }

        }

        public async Task<List<DailyGoal>> GetDailyGoalsByPatientId(long patientId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoals");


            try
            {
                var response = await _client.GetAsync(_requestUri);
                var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();

                return dailyGoals;
            }
            catch (HttpRequestException)
            {
                return new List<DailyGoal>();
            }

        }
    }
}