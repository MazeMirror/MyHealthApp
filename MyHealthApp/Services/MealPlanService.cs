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

namespace MyHealthApp.Services
{
    public class MealPlanService
    {
        private static Lazy<MealPlanService> _instance = new Lazy<MealPlanService>(() => new MealPlanService());
        public static MealPlanService Instance => _instance.Value;

        private Uri _requestUri;
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _settingsJson;

        MealPlanService()
        {
            _client = new HttpClient();
            _settingsJson = new JsonSerializerSettings
            { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public async Task<List<MealPlan>> GetMealPlansByPatientId(long patientId)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/mealPlans");
            var response = await _client.GetAsync(_requestUri);

            var mealPlans = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<MealPlan>>(response.Content.ReadAsStringAsync().Result)
                : new List<MealPlan>();

            return mealPlans;
        }
    }
}
