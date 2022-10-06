using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyHealthApp.Services
{
    public class WeeklyGoalService
    {
        private static Lazy<WeeklyGoalService> _instance = new Lazy<WeeklyGoalService>(()=> new WeeklyGoalService());
        public static WeeklyGoalService Instance => _instance.Value;
        
        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        WeeklyGoalService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
        
        public async Task<List<WeeklyGoal>> GetWeeklyGoalsByPatientId(long patientId)
        {
            DateTime date = DateTime.Today;
            int day = (int) date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day== 0 ? day : 7 - day));
            
            //Obtenemos los weeklyGoals correspondientes a esta semana
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}" +
                                  $"/weeklyGoals?startDate={monday.ToString("d",CultureInfo.InvariantCulture)}" +
                                  $"&endDate={sunday.ToString("d",CultureInfo.InvariantCulture)}");
            var response = await _client.GetAsync(_requestUri);
            
            var weeklyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<WeeklyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<WeeklyGoal>();
            
            return weeklyGoals;
        }

        public async Task<WeeklyGoal> PostWeeklyGoalByPatientId(long patientId, WeeklyGoal weeklyGoal)
        {
            DateTime date = DateTime.Today;
            int day = (int) date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day== 0 ? day : 7 - day));

            weeklyGoal.StartDate = monday;
            weeklyGoal.EndDate = sunday;
            
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/weeklyGoal");
            var json = JsonConvert.SerializeObject(weeklyGoal, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);

            return response.StatusCode == HttpStatusCode.Created ?  JsonConvert.DeserializeObject<WeeklyGoal>(response.Content.ReadAsStringAsync().Result)
                : null;
        }

        public async Task<WeeklyGoal> PutWeeklyGoalByPatientId(long patientId, WeeklyGoal weekly)
        {
            DateTime date = DateTime.Today;
            int day = (int)date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day == 0 ? day : 7 - day));

            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/weeklyGoalId/{weekly.Id.ToString()}");
            var json = JsonConvert.SerializeObject(weekly, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _client.PutAsync(_requestUri, contentJson);

            return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<WeeklyGoal>(response.Content.ReadAsStringAsync().Result)
                : null;
        }

        public async Task<WeeklyGoal> DeleteWeeklyGoalByPatientId(long patientId, WeeklyGoal weekly)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/weeklyGoalId/{weekly.Id.ToString()}");

            var response = await _client.DeleteAsync(_requestUri);

            return null;
        }

        /*public async Task<List<WeeklyGoal>> GetWeeklyGoalsByPatientIdAndDate(long patientId, DateTime now)
        {
            //WeeklyGoal aun no tiene filtro por fecha
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/weeklyGoals?date={now.ToString("d",CultureInfo.InvariantCulture)}");
            
            var response = await _client.GetAsync(_requestUri);
            
            var weeklyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<WeeklyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<WeeklyGoal>();
            
            return weeklyGoals;
        }*/
    }
}