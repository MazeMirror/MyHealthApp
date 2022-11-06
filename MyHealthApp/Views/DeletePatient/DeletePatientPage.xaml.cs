using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using MyHealthApp.Views.DeletePatient.SuccessfulMessage;

namespace MyHealthApp.Views.DeletePatient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletePatientPage : Popup
    {
        private long specialist_id;
        private long patient_id;
        private Profile _profile;

        public DeletePatientPage(long specialist_id, long patient_id , Profile profile)
        {
            InitializeComponent();
            this.specialist_id = specialist_id;
            this.patient_id = patient_id;
            this._profile = profile;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void DeleteObjectiveDaily_Clicked(object sender, EventArgs e)
        {
            var specialistProfile = await App.SqLiteDb.GetProfileAsync();
            var specialist = await SpecialistService.Instance.GetSpecialistByProfileId(specialistProfile.Id);

            var resp = await SpecialistService.Instance.UnassignSpecialistWitPatient(specialist_id, patient_id);
            if (resp)
            {
                PatientsListPage._viewModel.DeletePatientProfileOnList(_profile);
                SpecialistHomePage.ViewModel.DeletePatientProfileOnList(_profile);
                Dismiss(2);
                Navigation.ShowPopup(new DeletedPatientSucc());
            }


            /*if (dailyGoalResponse == HttpStatusCode.OK)
            {
                //ELIMINAR DE LISTA.....de dailyGoals
                PatientDetailsPage.DailyGoalsViewModel.DeleteDailyGoalOnList(_dailyGoal);
                Dismiss(2);
                Navigation.ShowPopup(new DeletedPatientSucc());
            
            else
            {
                await DisplayAlert("Importante", "No se pudo desasignar de este paciente, intentelo de nuevo", "Ok");
            }
            }*/

        }
    }
}