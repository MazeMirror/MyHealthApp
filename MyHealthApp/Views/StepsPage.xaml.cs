using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage
    {
        private readonly TimeSpan _second = TimeSpan.FromSeconds(5);
        private StepsViewModel _stepsViewModel;
        public StepsPage()
        {
            _stepsViewModel = new StepsViewModel();
            InitializeComponent();
            Setup();
            //SetBindingContext();
        }

        private void Setup()
        {
            Device.StartTimer(_second, () => {
                _stepsViewModel.UpdateInfo();
                LabelSteps.Text = _stepsViewModel.TodayStepCount.ToString();
                return true;
            });
        }

        
    }
}