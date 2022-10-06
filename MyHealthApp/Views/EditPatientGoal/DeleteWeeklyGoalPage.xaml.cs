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

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteWeeklyGoalPage : Popup
    {
        private WeeklyGoal _weeklyGoal;
        private long _patientId;
        public DeleteWeeklyGoalPage(WeeklyGoal weeklyGoal)
        {
            InitializeComponent();
            _weeklyGoal = weeklyGoal;
        }
        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void DeleteObjectiveWeekly_Clicked(object sender, EventArgs e)
        {
            _patientId = _weeklyGoal.PatientId;
            var weeklyGoalResponse = await WeeklyGoalService.Instance.DeleteWeeklyGoalByPatientId(_patientId, _weeklyGoal);
            
            if (weeklyGoalResponse == HttpStatusCode.OK)
            {
                PatientDetailsPage.WeeklyGoalViewModel.DeleteWeeklyGoalOnList(_weeklyGoal);
                Dismiss(2);
                Navigation.ShowPopup(new DeletedGoalPage());
            }
            

        }
    }
}