using System;
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
        
        public async Task<List<DailyGoal>> GetDailyGoalsByPatientId(long patientId)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoals");
            var response = await _client.GetAsync(_requestUri);
            
            var dailyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<DailyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<DailyGoal>();
            
            return dailyGoals;
        }

        public async Task<DailyGoal> PostDailyGoalByPatientId(long patientId, DailyGoal dailyGoal)
        {
            _requestUri = new Uri($"https://myhealth-1664161814891.azurewebsites.net/api/patient/{patientId.ToString()}/dailyGoal");
            var json = JsonConvert.SerializeObject(dailyGoal, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);

            return response.StatusCode == HttpStatusCode.Created ?  JsonConvert.DeserializeObject<DailyGoal>(response.Content.ReadAsStringAsync().Result)
                : null;
        }
    }
}