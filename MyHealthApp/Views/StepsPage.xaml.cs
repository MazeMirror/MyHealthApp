using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.ViewModels;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage
    {
        private readonly TimeSpan _second = TimeSpan.FromSeconds(5);
        private StepsViewModel _stepsViewModel;
        DailyGoal firstStepDg;
        public StepsPage(DailyGoal dailyGoal)
        {
            InitializeComponent();
            
            if (dailyGoal != null && Windesheart.PairedDevice != null)
            {
                firstStepDg = dailyGoal;
                _stepsViewModel = new StepsViewModel();
                LabelSteps.BindingContext = _stepsViewModel;
                LabelSteps.SetBinding(Label.TextProperty,"TodayStepCount");
                Setup();
            }
            
            //SetBindingContext();
        }
        
        

        private void RefreshViewSteps()
        {
            _stepsViewModel.UpdateInfo();

            if (_stepsViewModel.TodayStepCount > 0 && firstStepDg != null)
            {
                firstStepDg.Progress = _stepsViewModel.TodayStepCount;
            }
        }

        private void Setup()
        {
            Device.StartTimer(_second, () => {
                _stepsViewModel.UpdateInfo();
                //LabelSteps.Text = _stepsViewModel.TodayStepCount.ToString();
                return true;
            });
        }

        
    }
}