using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.AddPatientPlan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMealPlanPage : ContentPage
    {
        private long _patientId;
        private String countDescription;
        public AddMealPlanPage(long patientId)
        {
            InitializeComponent();
            _patientId = patientId;
            CounterText.Text = 0.ToString();
            PlanDescription.Text = "*Lunes: --- \n\n*Martes: --- \n\n*Miercoles: --- " +
                "\n\n*Jueves: --- \n\n*Viernes: --- \n\n*Sabado: --- \n\n*Domingo: ---";
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddPlanButton_OnClicked(object sender, EventArgs e)
        {
            if(PlanName.Text == null || PlanDescription.Text == null)
            {
                await DisplayAlert("Advertencia", "Los campos estan vacios", "Ok");
                return;
            }

            var mealPlan = new MealPlan()
            {
                Name = PlanName.Text.ToString(),
                Description = PlanDescription.Text.ToString(),
            };

            var mealPlanResponse = await MealPlanService.Instance.PostMealPlanByPatientId(_patientId, mealPlan);

            if (mealPlanResponse == null)
            {
                await DisplayAlert("Error", "No se pudo crear el Meal Plan para este paciente", "Ok");
                return;
            }

            PatientDetailsPage.MealPlansViewModel.AddMealPlanToList(mealPlanResponse);
            await Navigation.PopAsync();
        }

        private void CountText(object sender, TextChangedEventArgs e)
        {
            countDescription = PlanDescription.Text.ToString();
            CounterText.Text = countDescription.Length.ToString();
        }
    }
}