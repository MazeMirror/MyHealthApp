using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteDailyGoalPage : ContentPage
    {
        private DailyGoal _dailyGoal;
        private long _patientId;
        public DeleteDailyGoalPage(DailyGoal dailyGoal)
        {
            InitializeComponent();
            _dailyGoal = dailyGoal;
        }
        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void DeleteObjectiveDaily_Clicked(object sender, EventArgs e)
        {
            _patientId = _dailyGoal.PatientId;
            var dailyGoalResponse = await DailyGoalService.Instance.DeleteDailyGoalByPatientId(_patientId, _dailyGoal);


            if (dailyGoalResponse == HttpStatusCode.OK)
            {
                //ELIMINAR DE LISTA.....de dailyGoals
                PatientDetailsPage.DailyGoalsViewModel.DeleteDailyGoalOnList(_dailyGoal);
                await Navigation.PushAsync(new DeletedGoalPage());
            }
            else
            {
                await DisplayAlert("Mensaje", "No se pudo eliminar el Daily Goal", "Ok");
            }

        }
    }
}