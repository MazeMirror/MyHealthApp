﻿using System;
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
        private ReportGoalsViewModel _reportGoalsViewModel;
        //private List<DailyGoal> _dailyGoals;
        public PatientReportPage(Profile profile, Patient patient)
        {
            InitializeComponent();
            _profile = profile;
            _patient = patient;
            
            _reportGoalsViewModel = new ReportGoalsViewModel();
            //_dailyGoals = new List<DailyGoal>();
            //InitReportGoalsVm();
            FlexLayoutDailyInform.BindingContext = _reportGoalsViewModel;
            LabelName.Text = _profile.Name;
            LabelLastname.Text = _profile.LastName;
            CalculateInformForDay(DatePickerDailyInform.Date);
        }

        private void InitReportGoalsVm()
        {
            //Pasos
            /*_dailyGoals.Add(new DailyGoal()
            {
                Progress = 400,
                Quantity = 400,
                ActivityId = 1
            });
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 400,
                Quantity = 600,
                ActivityId = 1
            });
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 0,
                Quantity = 800,
                ActivityId = 1
            });
            
            //Calorias
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 3000,
                Quantity = 3000,
                ActivityId = 2
            });
            
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 3000,
                Quantity = 3500,
                ActivityId = 2
            });
            
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 0,
                Quantity = 4000,
                ActivityId = 2
            });
            
            //Distancias
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 435,
                Quantity = 435,
                ActivityId = 3
            });
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 550,
                Quantity = 550,
                ActivityId = 3
            });
            _dailyGoals.Add(new DailyGoal()
            {
                Progress = 600,
                Quantity = 800,
                ActivityId = 3
            });*/
            
        }

        private async void CalculateInformForDay(DateTime dateTime)
        {
            var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(_patient.Id, dateTime);
            
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
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ButtonDay_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(1);
            
            
        }

        private void ButtonWeek_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(2);
        }

        private void ButtonMonth_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(3);
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

        private void LabelDateChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDailyInform.Focus();
        }

        private void DatePickerDailyInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _reportGoalsViewModel.ClearElementsCollection();
            CalculateInformForDay(DatePickerDailyInform.Date);
        }
    }
}