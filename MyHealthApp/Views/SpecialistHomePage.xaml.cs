using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialistHomePage : ContentPage
    {
        public static PatientsProfilesViewModel ViewModel;
        public SpecialistHomePage()
        {
            InitializeComponent();
            ViewModel = new PatientsProfilesViewModel();
            
            StackLayoutPatients.BindingContext = ViewModel;
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
                    var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(patient.Id,DateTime.Now);
                    var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(patient.Id);
                    await Navigation.PushAsync(new PatientDetailsPage(profile,patient,dailyGoals,weeklyGoals));
                }
                
            }
        }
    }
}