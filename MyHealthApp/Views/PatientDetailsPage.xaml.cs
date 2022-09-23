﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Helpers;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using ProgressRingControl.Forms.Plugin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientDetailsPage : ContentPage
    {
        private readonly Profile _profile;
        private SlProfileDetailsViewModel _model;
        private readonly PatientDailyGoalsViewModel _dailyGoalsViewModel;
        private readonly Patient _patient;

        public PatientDetailsPage(Profile profile,Patient patient, List<DailyGoal> dailyGoals)
        {
            InitializeComponent();
            _profile = profile.CreateDeepCopy();
            _patient = patient.CreateDeepCopy();
            LabelName.Text = _profile.Name;
            LabelLastname.Text = _profile.LastName;
            
            _dailyGoalsViewModel = new PatientDailyGoalsViewModel(dailyGoals);
            
            GetDailyGoalsInformation();
            GetDailyGoalsStepAndWalk();
            GetDataInformation();
        }

        private void GetDailyGoalsStepAndWalk()
        {
            DailyGoal firstStepDg;
            DailyGoal firstWalkDg;
            try
            {
                firstStepDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity).ToList().First();
                LabelProgressSteps.Text = firstStepDg.Progress.ToString(CultureInfo.CurrentCulture);
                LabelGoalSteps.Text = "/"+firstStepDg.Quantity.ToString(CultureInfo.CurrentCulture);
                ProgressRingSteps.Progress = firstStepDg.Percentage;
            }
            catch (InvalidOperationException e)
            {
                FlexLayoutRingsToday.Children.Remove(ProgressRingSteps);
                FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingSteps);
                FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
            }

            try
            {
                firstWalkDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 2 && e.Progress < e.Quantity).ToList().First();
                LabelProgressWalk.Text = firstWalkDg.Progress.ToString(CultureInfo.CurrentCulture)+" min";
                LabelGoalWalk.Text = "/"+firstWalkDg.Quantity.ToString(CultureInfo.CurrentCulture)+" min";
                ProgressRingWalk.Progress = firstWalkDg.Percentage;
            }
            catch (InvalidOperationException e1)
            {
                FlexLayoutRingsToday.Children.Remove(ProgressRingWalk);
                FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingWalk);
                FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
                //FlexLayoutRingsToday.Children.Remove(ProgressRingWalk);
            }

            if (FlexLayoutRingsToday.Children.Count == 0)
            {
                FlexLayoutRingsToday.Children.Add(new Label()
                {
                    Text = "No hay objetivos diarios o pendientes",
                    FontFamily = "ArchivoRegular",
                    Padding = new Thickness(0,20,0,50),
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 14
                });
            }
            
            
          
            
        }

        private void GetDailyGoalsInformation()
        {
            
           
            //_dailyGoalsViewModel.AddDailyGoalToList(new DailyGoal(){Id = 1,Progress = 200,Quantity = 350,ActivityId = 1,PatientId = 1,Percentage = 0});
            //_dailyGoalsViewModel.AddDailyGoalToList(new DailyGoal(){Id = 1,Progress = 20,Quantity = 150,ActivityId = 1,PatientId = 1,Percentage = 0});
            //_dailyGoalsViewModel.AddDailyGoalToList(new DailyGoal(){Id = 1,Progress = 5000,Quantity = 6500,ActivityId = 1,PatientId = 1,Percentage = 0});

            
           //var responseList = await DailyGoalService.Instance.GetDailyGoalsByPatientId(patient.Id);
            
            
           
            /*foreach (var item in responseList)
            {
                Esto asigna pero se va a segundo plano o algo asi
                _dailyGoalsViewModel.AddDailyGoalToList(item);
            }*/ 
            
            _dailyGoalsViewModel.AdjustProgressPercentages();
           
            
            var completedGoals = _dailyGoalsViewModel.DailyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelDgCompleted.Text = $"{completedGoals.ToString()} / {_dailyGoalsViewModel.DailyGoals.Count.ToString()}";
            
            
            /*DataTemplate progressRingTemplate = new DataTemplate(() =>
            {
                var progressRing = new ProgressRing()
                {
                    HeightRequest = 30.0,
                    WidthRequest = 30.0,
                    RingProgressColor = Color.FromHex("#2F6A88"),
                    RingBaseColor = Color.FromHex("#CEE5EE"),
                    RingThickness = 5,
                    Margin = new Thickness(0,0,5,5),
                };

                ValueToDoubleConverter valueToDoubleConverter = new ValueToDoubleConverter();
                
                progressRing.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,valueToDoubleConverter);
                
                
                return progressRing;
            });*/
            
            StackLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            FlexLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            //BindableLayout.SetItemsSource(FlexLayoutDailyGoals,_dailyGoalsViewModel.DailyGoals);
            //BindableLayout.SetItemTemplate(FlexLayoutDailyGoals,progressRingTemplate);
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void GetDataInformation()
        {
            //var patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            var user = await UserService.Instance.GetUserById(_profile.UserId);
            _model = new SlProfileDetailsViewModel(_profile, _patient, ConvertToEntity.ConvertToUserEntity(user));
            StackLayoutProfileDetails.BindingContext = _model;
        }
    }
}