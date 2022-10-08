using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.Collections;
using MyHealthApp.Models;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentDayReportPage : ContentPage
    {
        private readonly ObservableCollection<DailyGoal> _dailyGoalsObservable;
        private readonly ReportGoalsViewModel _reportGoalsViewModel;
        public CurrentDayReportPage(ObservableCollection<DailyGoal> dailyGoals)
        {
            InitializeComponent();
            _dailyGoalsObservable = dailyGoals;
            _reportGoalsViewModel = new ReportGoalsViewModel();
            FlexLayoutDailyInform.BindingContext = _reportGoalsViewModel;
            FrameDailyGoals.BindingContext = _dailyGoalsObservable;
            CalculateInformForToday();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private void CalculateInformForToday()
        {
            var dailyGoals = new List<DailyGoal>();

            foreach (var item in _dailyGoalsObservable)
            {
                dailyGoals.Add(item);
            }
            
            var listSteps = dailyGoals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = dailyGoals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = dailyGoals.Where(dg => dg.ActivityId == 3).ToList();

            double maxQuantity = 0;
            double maxProgress = 0;
            
            if (!listSteps.IsEmpty())
            {
                foreach (var item in listSteps)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 1
                });
            }
            
            //reiniciamos maximos
            maxQuantity = 0;
            maxProgress = 0;

            if (!listCalories.IsEmpty())
            {
                foreach (var item in listCalories)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 2
                });
            }
            
            //reiniciamos maximos
            maxQuantity = 0;
            maxProgress = 0;

            if (!listDistances.IsEmpty())
            {
                foreach (var item in listDistances)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }
    }
}