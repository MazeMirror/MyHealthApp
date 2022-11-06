using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Views.SuccesfulMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.AddPatientPlan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMealPlanPage : Popup
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
            if(PlanName.Text == null || PlanName.Text == "")
            {
                Navigation.ShowPopup(new SMPage("Advertencia", "El Nombre esta vacio", false, false));
                //await DisplayAlert("Advertencia", "El Nombre esta vacio", "Ok");
                return;
            }
            
            if(PlanDescription.Text == null || PlanDescription.Text == "")
            {
                Navigation.ShowPopup(new SMPage("Advertencia", "La Descripcion esta vacia", false, false));
                //await DisplayAlert("Advertencia", "La Descripcion esta vacia", "Ok");
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
                Navigation.ShowPopup(new SMPage("Error", "No se pudo crear el Meal Plan para este paciente", false, false));
                //await DisplayAlert("Error", "No se pudo crear el Meal Plan para este paciente", "Ok");
                return;
            }

            PatientDetailsPage.MealPlansViewModel.AddMealPlanToList(mealPlanResponse);
            //await Navigation.PopAsync();
            Dismiss(2);
            Navigation.ShowPopup(new SMPage("Añadido exitosamente", "El plan alimenticio se ha agregado", true, false));
        }

        private void CountText(object sender, TextChangedEventArgs e)
        {
            countDescription = PlanDescription.Text.ToString();
            CounterText.Text = countDescription.Length.ToString();
        }
    }
}