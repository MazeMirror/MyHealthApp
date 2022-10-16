using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteProfilePage : Popup
    {
        public static Profile LocalProfile;
        public Specialist specialist;
        public Patient patient;
        public User user;
        private List<Specialist> listSpecialists;
        private Specialist specialistId;

        private List<Patient> listPatients;
        private List<DailyGoal> listDailyGoals;
        private List<WeeklyGoal> listWeeklyGoals;
        private List<MealPlan> listMealPLans;


        public DeleteProfilePage(Profile profile)
        {
            InitializeComponent();
            LocalProfile = profile;
            patient = new Patient();

            listPatients = new List<Patient>();
            listDailyGoals = new List<DailyGoal>();
            listWeeklyGoals = new List<WeeklyGoal>();
            listMealPLans = new List<MealPlan>();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void DeleteAccount_Clicked(object sender, EventArgs e)
        {
            if (LocalProfile.RoleId == 1)
            {
                patient = await PatientService.Instance.GetPatientByProfileId(LocalProfile.Id);
                listSpecialists = (List<Specialist>)await PatientService.Instance.GetSpecialistByPatientId(patient.Id);
                specialistId = listSpecialists.Last();

                DeleteUserAsAdmin(patient, specialistId);
            }

            Dismiss(1);
            Application.Current.MainPage = new NavigationPage(new WelcomePage());
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

            await ProfileService.Instance.DeleteProfileById(LocalProfile.Id);

            //cuarto eliminar user

            await UserService.Instance.DeleteUserById(LocalProfile.UserId);
        }
    }
}