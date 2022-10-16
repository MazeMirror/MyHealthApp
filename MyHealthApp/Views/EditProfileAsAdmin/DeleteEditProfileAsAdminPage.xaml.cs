using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyHealthApp.Views.AddPatientPlan;
using ProgressRingControl.Forms.Plugin;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteEditProfileAsAdminPage : Popup
    {
        private Profile _profile;

        private Patient patient;
        private Specialist specialist;

        private Specialist specialistId;

        private List<Patient> listPatients;
        private List<Specialist> listSpecialists;
        private List<DailyGoal> listDailyGoals;
        private List<WeeklyGoal> listWeeklyGoals;
        private List<MealPlan> listMealPLans;


        public DeleteEditProfileAsAdminPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            patient = new Patient();
            
            listPatients = new List<Patient>();
            listDailyGoals = new List<DailyGoal>();
            listWeeklyGoals = new List<WeeklyGoal>();
            listMealPLans = new List<MealPlan>();
        }

        /*private async void GetPatientIdData()
        {
            //listPatients = 

            Patient pa = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            //prueba1.Text = listPatients.Last().Id.ToString();
            //prueba2.Text = listPatients.Last().ProfileId.ToString();

            foreach (Patient patient in listPatients)
            {
                if (patient.ProfileId.ToString() == _profile.Id.ToString())
                {
                    prueba1.Text = _patientId.Id.ToString();
                    prueba2.Text = patient.Id.ToString();
                    _patientId = patient;
                    //prueba3.Text = _patientId.Id.ToString();
                    //prueba4.Text = patient.Id.ToString();
                }
            }

        }*/

        private async void DeleteUser_Clicked(object sender, EventArgs e)
        {

            if(_profile.RoleId == 1)
            {
                patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
                listSpecialists = (List<Specialist>)await PatientService.Instance.GetSpecialistByPatientId(patient.Id);
                specialistId = listSpecialists.Last();

                DeleteUserAsAdmin(patient, specialistId);
            }

            if(_profile.RoleId == 2)
            {
                specialist = await SpecialistService.Instance.GetSpecialistByProfileId(_profile.Id);
                var listPatientsToDelete = await SpecialistService.Instance.GetPatientsBySpecialistId(specialist.Id);

                //prueba1.Text = listSpecialist.Last().Id.ToString();
                //prueba2.Text = _profile.RoleId.ToString();

                foreach(Patient patientToDelete in listPatientsToDelete)
                {
                    DeleteUserAsAdmin(patientToDelete, specialist);
                }

            }
            Application.Current.MainPage = new NavigationPage(new AdminHomePage());

            //await Navigation.PushAsync(new AdminHomePage());

        }

        private async void DeleteUserAsAdmin(Patient patientToDelete, Specialist specialist)
        {
            //primero eliminar daily, weekly, meals
            listDailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(patientToDelete.Id);
            listWeeklyGoals = await WeeklyGoalService.Instance.GetAllWeeklyGoalsByPatientId(patientToDelete.Id);
            listMealPLans = await MealPlanService.Instance.GetMealPlansByPatientId(patientToDelete.Id);

            foreach (DailyGoal dailyGoal in listDailyGoals)
            {
                if (listDailyGoals != null)
                {
                    await DailyGoalService.Instance.DeleteDailyGoalByPatientId(patientToDelete.Id, dailyGoal);
                }
            }

            foreach (WeeklyGoal weeklyGoal in listWeeklyGoals)
            {
                if (listWeeklyGoals != null)
                {
                    await WeeklyGoalService.Instance.DeleteWeeklyGoalByPatientId(patientToDelete.Id, weeklyGoal);
                }
            }

            foreach (MealPlan mealPlan in listMealPLans)
            {
                if (listMealPLans != null)
                {
                    await MealPlanService.Instance.DeleteMealPlanByPatientId(patientToDelete.Id, mealPlan);
                }
            }

            //segundo eliminar paciente

            var Specialist = await PatientService.Instance.GetSpecialistByPatientId(patientToDelete.Id);
            await SpecialistService.Instance.DeletePatientBySpecialistId(specialist.Id, patientToDelete.Id);

            await PatientService.Instance.DeletePatient(patientToDelete.Id);

            //tercero eliminar profile

            await ProfileService.Instance.DeleteProfileById(_profile.Id);

            //cuarto eliminar user

            await UserService.Instance.DeleteUserById(_profile.UserId);
        }
        
        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage());

            //await Navigation.PushAsync(new LoginPage());
        }
    }
}