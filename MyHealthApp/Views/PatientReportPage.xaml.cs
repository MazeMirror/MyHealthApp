using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.Collections;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientReportPage : ContentPage
    {
        private readonly Profile _profile;
        private readonly Patient _patient;
        private readonly ReportGoalsViewModel _reportGoalsDailyViewModel;

        private readonly ReportGoalsViewModel _reportGoalsWeeklyViewModel;

        private readonly ReportGoalsViewModel _reportGoalsMonthlyViewModel;

        private PatientDailyGoalsViewModel _dailyGoalsViewModel;

        private PatientWeeklyGoalViewModel _weeklyGoalViewModel;
        //private List<DailyGoal> _dailyGoals;
        public PatientReportPage(Profile profile, Patient patient)
        {
            InitializeComponent();
            _profile = profile;
            _patient = patient;
            
            _reportGoalsDailyViewModel = new ReportGoalsViewModel();
            _reportGoalsWeeklyViewModel = new ReportGoalsViewModel();
            _reportGoalsMonthlyViewModel = new ReportGoalsViewModel();
            
            _dailyGoalsViewModel = new PatientDailyGoalsViewModel();
            _weeklyGoalViewModel = new PatientWeeklyGoalViewModel();

            InitConfigurations();
            //_dailyGoals = new List<DailyGoal>();
            //InitReportGoalsVm();
        }

        private void InitConfigurations()
        {
            //Perfil de paciente
            LabelName.Text = _profile.Name;
            LabelLastname.Text = _profile.LastName;
            //Layouts de informe
            FlexLayoutDailyInform.BindingContext = _reportGoalsDailyViewModel;
            FlexLayoutWeeklyInform.BindingContext = _reportGoalsWeeklyViewModel;
            FlexLayoutMonthlyInform.BindingContext = _reportGoalsMonthlyViewModel;
            
            //Daily Inform
            CalculateInformForDay(DatePickerDailyInform.Date);
            DatePickerDailyInform.MaximumDate = DateTime.Today;
            
            //Weekly inform
            DateTime date = DateTime.Today;
            int day = (int)date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day == 0 ? day : 7 - day));
            
            DatePickerDateWeekStartInform.Date = monday;
            DatePickerDateWeekFinishInform.Date = sunday;
            
            DatePickerDateWeekStartInform.MaximumDate = DatePickerDateWeekFinishInform.Date.AddDays(-1);
            DatePickerDateWeekFinishInform.MinimumDate = DatePickerDateWeekStartInform.Date.AddDays(1);
            
            
            //Monthly inform
            CalculateInformForMonth(DatePickerMonthInform.Date);
            
            
            //Layout daily
            StackLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            //Layout weekly
            StackLayoutWeeklyGoals.BindingContext = _weeklyGoalViewModel;
        }
        
        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ButtonDay_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(1);
            ChangeFiltersByButtonClicked(1);

        }

        private void ButtonWeek_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(2);
            ChangeFiltersByButtonClicked(2);
            
            //_reportGoalsWeeklyViewModel.ClearElementsCollection();
            
        }

        private void ButtonMonth_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(3);
            ChangeFiltersByButtonClicked(3);
        }


        private void ChangeColorButtonsOnClick(int id)
        {
            switch (id)
            {
                case 1:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonDay.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonDay.TextColor = Color.White;
                        ButtonWeek.BackgroundColor = Color.White;
                        ButtonWeek.TextColor = Color.Black;
                        ButtonMonth.BackgroundColor = Color.White;
                        ButtonMonth.TextColor = Color.Black;
                    });
                }; break;
                case 2:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonWeek.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonWeek.TextColor = Color.White;
                        ButtonDay.BackgroundColor = Color.White;
                        ButtonDay.TextColor = Color.Black;
                        ButtonMonth.BackgroundColor = Color.White;
                        ButtonMonth.TextColor = Color.Black;
                    });   
                }; break;
                case 3:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonMonth.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonMonth.TextColor = Color.White;
                        ButtonWeek.BackgroundColor = Color.White;
                        ButtonWeek.TextColor = Color.Black;
                        ButtonDay.BackgroundColor = Color.White;
                        ButtonDay.TextColor = Color.Black;
                    });
                }; break;
            }
        }

        private void ChangeFiltersByButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StackLayoutDailyInform.IsVisible = true;
                        FrameDailyGoals.IsVisible = true;
                        
                        StackLayoutWeeklyInform.IsVisible = false;
                        FrameWeeklyGoals.IsVisible = false;
                        
                        StackLayoutMonthlyInform.IsVisible = false;
                    });
                    
                } ; break;
                case 2:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StackLayoutDailyInform.IsVisible = false;
                        FrameDailyGoals.IsVisible = false;
                        
                        StackLayoutWeeklyInform.IsVisible = true;
                        FrameWeeklyGoals.IsVisible = true;

                        StackLayoutMonthlyInform.IsVisible = false;
                    });
                    
                } ; break;
                case 3:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StackLayoutDailyInform.IsVisible = false;
                        FrameDailyGoals.IsVisible = false;
                        
                        StackLayoutWeeklyInform.IsVisible = false;
                        FrameWeeklyGoals.IsVisible = false;

                        StackLayoutMonthlyInform.IsVisible = true;
                    });
                    
                } ; break;
            }
        }


        #region Daily Informs

        private void LabelDateChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDailyInform.Focus();
        }

        private void DatePickerDailyInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _reportGoalsDailyViewModel.ClearElementsCollection();
            CalculateInformForDay(DatePickerDailyInform.Date);
        }
        
        private async void CalculateInformForDay(DateTime dateTime)
        {
            var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(_patient.Id, dateTime);
            
            _dailyGoalsViewModel.ClearDailyGoalList();
            foreach (var item in dailyGoals)
            {
                _dailyGoalsViewModel.AddDailyGoalToList(item);
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
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }

        #endregion
        


        #region Weekly Informs

        /*void InitWeeklyReport()
        {
            var item1 = new ReportGoal()
            {
                Progress = 850,
                Quantity = 950,
                ActivityId = 1,
                ImageSource = "",
                Description = "",
            };

            var item2 = new ReportGoal()
            {
                Progress = 1050,
                Quantity = 1550,
                ActivityId = 2,
                ImageSource = "",
                Description = "",
            };

            var item3 = new ReportGoal()
            {
                Progress = 550,
                Quantity = 750,
                ActivityId = 3,
                ImageSource = "",
                Description = "",
            };
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item1);
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item2);
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item3);
            
        }*/
        
        
        private void DatePickerDateWeekStartInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DatePickerDateWeekFinishInform.MinimumDate = DatePickerDateWeekStartInform.Date.AddDays(1);
            _reportGoalsWeeklyViewModel.ClearElementsCollection();
            CalculateInformForWeek(DatePickerDateWeekStartInform.Date,DatePickerDateWeekFinishInform.Date);
        }
        
        private void LabelDate1WeekChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDateWeekStartInform.Focus();
        }
        
        private void DatePickerDateWeekFinishInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DatePickerDateWeekStartInform.MaximumDate = DatePickerDateWeekFinishInform.Date.AddDays(-1);
            _reportGoalsWeeklyViewModel.ClearElementsCollection();
            CalculateInformForWeek(DatePickerDateWeekStartInform.Date,DatePickerDateWeekFinishInform.Date);
        }
        
        private void LabelDate2WeekChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDateWeekFinishInform.Focus();
        }
        
        private async void CalculateInformForWeek(DateTime start, DateTime end)
        {
            var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientIdAndDates(_patient.Id, start,end);
            
            _weeklyGoalViewModel.ClearWeeklyGoalList();
            foreach (var item in weeklyGoals)
            {
                _weeklyGoalViewModel.AddWeeklyToList(item);
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
                
                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }

        #endregion


        #region MonthInform

        private void DatePickerMonthInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _reportGoalsMonthlyViewModel.ClearElementsCollection();
            CalculateInformForMonth(DatePickerMonthInform.Date);
            //var firstDateOfMonth;
        }
        
        private async void CalculateInformForMonth(DateTime dateTime)
        {
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var goals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDates(_patient.Id,firstDayOfMonth,lastDayOfMonth);
            
            
            var listSteps = goals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = goals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = goals.Where(dg => dg.ActivityId == 3).ToList();

            double maxQuantity = 0;
            double maxProgress = 0;
            
            if (!listSteps.IsEmpty())
            {
                foreach (var item in listSteps)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
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
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }
        
        private void LabelMonthChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerMonthInform.Focus();
        }

        
        #endregion

       
    }
}