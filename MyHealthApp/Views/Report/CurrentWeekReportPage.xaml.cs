using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public partial class CurrentWeekReportPage : ContentPage
    {
        private readonly ObservableCollection<WeeklyGoal> _weeklyGoalsObservable;
        private readonly ReportGoalsViewModel _reportGoalsViewModel;
        public CurrentWeekReportPage(ObservableCollection<WeeklyGoal> weeklyGoals)
        {
            InitializeComponent();
            _weeklyGoalsObservable = weeklyGoals;
            _reportGoalsViewModel = new ReportGoalsViewModel();
            FlexLayoutWeeklyInform.BindingContext = _reportGoalsViewModel;
            FrameWeeklyGoals.BindingContext = _weeklyGoalsObservable;
            
            DateTime date = DateTime.Today;
            int day = (int)date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day == 0 ? day : 7 - day));

            LabelCurrentWeekStart.Text = monday.ToString("d MMM", new CultureInfo("es-ES"));
            LabelCurrentWeekEnd.Text = sunday.ToString("d MMM", new CultureInfo("es-ES"));
            
            
            
            
            CalculateInformForCurrentWeek();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void CalculateInformForCurrentWeek()
        {
            var weeklyGoals = new List<WeeklyGoal>();

            foreach (var item in _weeklyGoalsObservable)
            {
                weeklyGoals.Add(item);
            }
            
            var listSteps = weeklyGoals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = weeklyGoals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = weeklyGoals.Where(dg => dg.ActivityId == 3).ToList();

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