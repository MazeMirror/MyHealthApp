using System;
using System.Collections.Generic;
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
            _requestUri = new Uri($"http://192.168.1.15:8383/api/patient/{patientId.ToString()}/weeklyGoals");
            var response = await _client.GetAsync(_requestUri);
            
            var weeklyGoals = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<WeeklyGoal>>(response.Content.ReadAsStringAsync().Result)
                : new List<WeeklyGoal>();
            
            return weeklyGoals;
        }

        public async Task<WeeklyGoal> PostWeeklyGoalByPatientId(long patientId, WeeklyGoal weeklyGoal)
        {
            _requestUri = new Uri($"http://192.168.1.15:8383/api/patient/{patientId.ToString()}/weeklyGoal");
            var json = JsonConvert.SerializeObject(weeklyGoal, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            var response = await _client.PostAsync(_requestUri, contentJson);

            return response.StatusCode == HttpStatusCode.Created ?  JsonConvert.DeserializeObject<WeeklyGoal>(response.Content.ReadAsStringAsync().Result)
                : null;
        }
    }
}