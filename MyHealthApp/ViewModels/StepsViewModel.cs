using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WindesHeartSDK;
using WindesHeartSDK.Models;

namespace MyHealthApp.ViewModels
{
    public class StepsViewModel : INotifyPropertyChanged
    {
        private int _todayStepCount = 0;

        public int TodayStepCount
        {
            get => _todayStepCount;
            private set
            {
                _todayStepCount = value;
                OnPropertyChanged();
            }
        }
       
        
        public async void UpdateInfo()
        { 
            _todayStepCount = await GetCurrentSteps(); 
            OnPropertyChanged();
        }

        private async Task<int> GetCurrentSteps()
        {
            //If device is connected
            if (Windesheart.PairedDevice != null && Windesheart.PairedDevice.IsAuthenticated())
            {
                //Read stepcount from device
                try
                {
                    StepData currentSteps = await Windesheart.PairedDevice.GetSteps();
                    return currentSteps.StepCount;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error while trying to get stepcount: " + e.Message);
                    Debug.WriteLine("Falling back to data from DB...");
                    return -1;
                }
            }

            return 0;
        }
        
        
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}