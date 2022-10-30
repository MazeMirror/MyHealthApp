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
using MyHealthApp.Views.SuccesfulMessage;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteDailyGoalPage : Popup
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
            Dismiss(1);
        }
        
        

        private async void DeleteObjectiveDaily_Clicked(object sender, EventArgs e)
        {
            _patientId = _dailyGoal.PatientId;
            var dailyGoalResponse = await DailyGoalService.Instance.DeleteDailyGoalByPatientId(_patientId, _dailyGoal);


            if (dailyGoalResponse == HttpStatusCode.OK)
            {
                //ELIMINAR DE LISTA.....de dailyGoals
                PatientDetailsPage.DailyGoalsViewModel.DeleteDailyGoalOnList(_dailyGoal);
                Dismiss(2);
                Navigation.ShowPopup(new SMPage(6));
            }
            
        }
    }
}