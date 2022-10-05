using MyHealthApp.Models;
using ProgressRingControl.Forms.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWeeklyGoalPage : ContentPage
    {
        private WeeklyGoal _weeklyGoal;
        public EditWeeklyGoalPage(WeeklyGoal weeklyGoal)
        {
            InitializeComponent();
            _weeklyGoal = weeklyGoal;
            LabelGoal.BindingContext = _weeklyGoal;
            ProgressRing.BindingContext = _weeklyGoal;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void UpdateWeeklyGoal_Clicked(object sender, EventArgs e)
        {
            WeeklyGoal weeklyGoal = _weeklyGoal;
            await Navigation.PushAsync(new UpdateWeeklyGoalPage(weeklyGoal));
        }
    }
}