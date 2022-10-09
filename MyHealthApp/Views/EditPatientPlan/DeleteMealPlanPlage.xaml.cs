using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyHealthApp.Views.EditPatientPlan.SuccessfullMessage;

namespace MyHealthApp.Views.EditPatientPlan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteMealPlanPlage : Popup
    {
        private MealPlan _mealPlan;
        private long _patientId;
        public DeleteMealPlanPlage(MealPlan mealPlan)
        {
            InitializeComponent();
            _mealPlan = mealPlan;  
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(2);
        }

        private async void DeleteMealPlan_Clicked(object sender, EventArgs e)
        {
            _patientId = _mealPlan.PatientId;
            var mealPlanResponse = await MealPlanService.Instance.DeleteMealPlanByPatientId(_patientId, _mealPlan);


            if (mealPlanResponse == HttpStatusCode.OK)
            {
                //ELIMINAR DE LISTA.....de dailyGoals
                PatientDetailsPage.MealPlansViewModel.DeleteMealPlanlOnList(_mealPlan);
                Dismiss(2);
                Navigation.ShowPopup(new DeletedMealPlan());
            }

        }
    }
}