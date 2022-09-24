using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services.MiBand;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.Register;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientHomePage : ContentPage
    {
        private readonly string _propertyKey = "LastConnectedDevice";
        private PatientDailyGoalsViewModel _dailyGoalsViewModel;
        private PatientWeeklyGoalViewModel _weeklyGoalViewModel;
        public PatientHomePage()
        {
            if (SuccessfulRegisterPage.DailyGoals != null && SuccessfulRegisterPage.DailyGoals != null)
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(SuccessfulRegisterPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(SuccessfulRegisterPage.WeeklyGoals);
            }
            else
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(LoginPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(LoginPage.WeeklyGoals);
            }
            
            InitializeComponent();
            GetGoalsInformation();
            GetDailyGoalsStepAndWalk();
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

        private void GetGoalsInformation()
        {
            _dailyGoalsViewModel.AdjustProgressPercentages();
            
            var completedDgGoals = _dailyGoalsViewModel.DailyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelDgCompleted.Text = $"{completedDgGoals.ToString()} / {_dailyGoalsViewModel.DailyGoals.Count.ToString()}";
            var completedWgGoals = _weeklyGoalViewModel.WeeklyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelWgCompleted.Text = $"{completedWgGoals.ToString()} / {_weeklyGoalViewModel.WeeklyGoals.Count.ToString()}";
            
            //Los dailyGoals
            FlexLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            
            //Los weeklyGoals
            FlexLayoutWeeklyGoals.BindingContext = _weeklyGoalViewModel;
      
        }


        protected override void OnAppearing()
        { 
            //App.RequestLocationPermission();
            if (Windesheart.PairedDevice == null)
                return;
           
        }
        
        private void SetApplicationProperties()
        {
            if (Windesheart.PairedDevice != null)
            {
                App.Current.Properties[_propertyKey] = Windesheart.PairedDevice.Uuid;
            }
        }

        //Handle Auto-connect to the last connected device with App-properties
        private async Task HandleAutoConnect()
        {
            var knownGuid = App.Current.Properties[_propertyKey].ToString();
            if (!string.IsNullOrEmpty(knownGuid))
            {
                var knownDevice = await Windesheart.GetKnownDevice(Guid.Parse(knownGuid));
                knownDevice.Connect(CallbackHandler.OnConnect);
            }
        }

        private async void RegisterSmartwatch_OnClicked(object sender, EventArgs e)
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new DevicePage());
            await Navigation.PushAsync(new DevicePage());
        }
    }
}