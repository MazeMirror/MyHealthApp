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
using MyHealthApp.Views.AddPatientGoal;
using MyHealthApp.Views.EditPatientGoal;
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
        public static  PatientDailyGoalsViewModel DailyGoalsViewModel;
        public static PatientWeeklyGoalViewModel WeeklyGoalViewModel;
        private readonly Patient _patient;
        private DailyGoal _firstStepDg;
        private DailyGoal _firstWalkDg;

        public PatientDetailsPage(Profile profile,Patient patient, List<DailyGoal> dailyGoals, List<WeeklyGoal> weeklyGoals)
        {
            InitializeComponent();
            _profile = profile.CreateDeepCopy();
            _patient = patient.CreateDeepCopy();
            LabelName.Text = _profile.Name;
            LabelLastname.Text = _profile.LastName;
            
            DailyGoalsViewModel = new PatientDailyGoalsViewModel(dailyGoals);
            WeeklyGoalViewModel = new PatientWeeklyGoalViewModel(weeklyGoals);
            
            GetGoalsInformation();
            
            GetPersonalDataInformation();
        }

        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                GetDailyGoalsStepAndWalk();
            });
        }

        private void GetDailyGoalsStepAndWalk()
        {
            
            try
            {
                _firstStepDg = DailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity).ToList().First();
                LabelProgressSteps.Text = _firstStepDg.Progress.ToString(CultureInfo.CurrentCulture);
                LabelGoalSteps.Text = "/"+_firstStepDg.Quantity.ToString(CultureInfo.CurrentCulture);
                ProgressRingSteps.Progress = _firstStepDg.Percentage;
                
                ProgressRingSteps.IsVisible = true;
                StackLayoutInfoRingSteps.IsVisible = true;
                
                LabelEmptyOfRings.IsVisible = false;
            }
            catch (InvalidOperationException e)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingSteps);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingSteps);
                ProgressRingSteps.IsVisible = false;
                StackLayoutInfoRingSteps.IsVisible = false;
                
                FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
            }

            try
            {
                _firstWalkDg = DailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 2 && e.Progress < e.Quantity).ToList().First();
                LabelProgressWalk.Text = _firstWalkDg.Progress.ToString(CultureInfo.CurrentCulture)+" min";
                LabelGoalWalk.Text = "/"+_firstWalkDg.Quantity.ToString(CultureInfo.CurrentCulture)+" min";
                ProgressRingWalk.Progress = _firstWalkDg.Percentage;
                
                ProgressRingWalk.IsVisible = true;
                StackLayoutInfoRingWalk.IsVisible = true;
                
                LabelEmptyOfRings.IsVisible = false;
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingWalk);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingWalk);
                ProgressRingWalk.IsVisible = false;
                StackLayoutInfoRingWalk.IsVisible = false;
                
                FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
                //FlexLayoutRingsToday.Children.Remove(ProgressRingWalk);
            }
            
            

            if (!StackLayoutInfoRingWalk.IsVisible && !StackLayoutInfoRingSteps.IsVisible)
            {
                LabelEmptyOfRings.IsVisible = true;
                /*FlexLayoutRingsToday.Children.Add(new Label()
                {
                    Text = "No hay objetivos diarios o pendientes",
                    FontFamily = "ArchivoRegular",
                    Padding = new Thickness(0,20,0,50),
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 14
                });*/
            }
            
            
          
            
        }

        private void GetGoalsInformation()
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
            
            //_dailyGoalsViewModel.AdjustProgressPercentages();
           
            
            //var completedGoals = DailyGoalsViewModel.DailyGoals.Count(goal => goal.Progress == goal.Quantity);
            //LabelDgCompleted.Text = $"{completedGoals.ToString()} / {DailyGoalsViewModel.DailyGoals.Count.ToString()}";
            
            
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
            LabelDgCompleted.BindingContext = DailyGoalsViewModel;
            StackLayoutDailyGoals.BindingContext = DailyGoalsViewModel;
            FlexLayoutDailyGoals.BindingContext = DailyGoalsViewModel;
            
            //Los weeklyGoals
            StackLayoutWeeklyGoals.BindingContext = WeeklyGoalViewModel;
            //BindableLayout.SetItemsSource(FlexLayoutDailyGoals,_dailyGoalsViewModel.DailyGoals);
            //BindableLayout.SetItemTemplate(FlexLayoutDailyGoals,progressRingTemplate);
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void GetPersonalDataInformation()
        {
            //var patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            var user = await UserService.Instance.GetUserById(_profile.UserId);
            _model = new SlProfileDetailsViewModel(_profile, _patient, ConvertToEntity.ConvertToUserEntity(user));
            StackLayoutProfileDetails.BindingContext = _model;
        }

        private async void LabelAddDailyGoal_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddDailyGoalPage(_patient.Id));
        }

        private async void LabelAddWeeklyGoal_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddWeeklyGoalPage(_patient.Id));
        }

        
        private async void SpecificDailyGoal_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var dailyGoal = param as DailyGoal;

                if (dailyGoal != null)
                {
                    await Navigation.PushAsync(new EditDailyGoalPage(dailyGoal));
                }
            }
        }

        private async void SpecificWeeklyGoal_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var weeklyGoal = param as WeeklyGoal;

                if (weeklyGoal != null)
                {
                    await Navigation.PushAsync(new EditWeeklyGoalPage(weeklyGoal));
                }
            }
        }
    }
}