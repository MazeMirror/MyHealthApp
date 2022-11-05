using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.SuccesfulMessage;
using Plugin.BluetoothLE;
using WindesHeartSDK;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialistHomePage : ContentPage
    {
        public static PatientsProfilesViewModel ViewModel;
        private bool _isTimerWorking = false;
        public SpecialistHomePage()
        {
            InitializeComponent();
            ViewModel = new PatientsProfilesViewModel();
            
            StackLayoutPatients.BindingContext = ViewModel;
            checkConnectionWifi();
        }

        private async void FramePatient_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var profile = param as Profile;

                if (profile != null)
                {
                    //BUG: El progress ring no renderiza del data template con data asíncrona
                    //Solucion parcial es esta
                    var patient = await PatientService.Instance.GetPatientByProfileId(profile.Id);
                    var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(patient.Id,DateTime.Today);
                    var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(patient.Id);
                    var mealPlans = await MealPlanService.Instance.GetMealPlansByPatientId(patient.Id);
                    await Navigation.PushAsync(new PatientDetailsPage(profile,patient,dailyGoals,weeklyGoals,mealPlans));
                }
                
            }
        }

        private void checkConnectionWifi()
        {
            //mostrara el mensaje cada 6 segs
            Device.StartTimer(new TimeSpan(0, 0, 6), () =>
            {

                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    _isTimerWorking = true;                   
                }
                else
                {
                    DisplayAlert("Importante", "No cuenta con conexion a internet, restablezca su conexión para continuar", "Aceptar");
                }
                return _isTimerWorking;
            });
        }     
    }
}