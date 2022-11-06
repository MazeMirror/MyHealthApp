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
            

            try {
                var response = await _client.GetAsync(_requestUri);
                var mealPlans = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<MealPlan>>(response.Content.ReadAsStringAsync().Result)
                : new List<MealPlan>();

                return mealPlans;
            }
            catch (HttpRequestException)
            {
                return new List<MealPlan>();
            }
            
        }

        public async Task<MealPlan> PostMealPlanByPatientId(long patientId, MealPlan mealPlan)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/mealPlans");
            var json = JsonConvert.SerializeObject(mealPlan, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.Created ? JsonConvert.DeserializeObject<MealPlan>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<MealPlan> PutMealPlanByPatientId(long patientId, MealPlan meal)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/mealPlans/{meal.Id.ToString()}");
            var json = JsonConvert.SerializeObject(meal, _settingsJson);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PutAsync(_requestUri, contentJson);

                return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<MealPlan>(response.Content.ReadAsStringAsync().Result)
                    : null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<HttpStatusCode> DeleteMealPlanByPatientId(long patientId, MealPlan meal)
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/patient/{patientId.ToString()}/mealPlans/{meal.Id.ToString()}");

            try {
                var response = await _client.DeleteAsync(_requestUri);

                return response.StatusCode;
            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.BadRequest;
            }
            
        }

        public async Task<List<MealPlan>> GetAllMealPlans()
        {
            _requestUri = new Uri($"https://myhealthnewapi.azurewebsites.net/api/mealPlan");

            try
            {
                var response = await _client.GetAsync(_requestUri);
                var mealPlans = response.StatusCode == HttpStatusCode.OK
                ? JsonConvert.DeserializeObject<List<MealPlan>>(response.Content.ReadAsStringAsync().Result)
                : new List<MealPlan>();

                return mealPlans;
            }
            catch (Exception)
            {
                return new List<MealPlan>();
            }

        }
    }
}
