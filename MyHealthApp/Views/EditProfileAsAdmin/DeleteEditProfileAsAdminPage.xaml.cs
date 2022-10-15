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

namespace MyHealthApp.Views.EditProfileAsAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteEditProfileAsAdminPage : Popup
    {
        private Profile _profile;
        private Patient _patientId;

        private Patient patient;

        private DailyGoal _dailyGoal;
        private WeeklyGoal _weeklyGoal;
        private MealPlan _mealPlan;
        private List<Patient> listPatients;
        private List<DailyGoal> listDailyGoals;
        private List<WeeklyGoal> listWeeklyGoals;
        private List<MealPlan> listMealPLans;


        public DeleteEditProfileAsAdminPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            _patientId = new Patient();

            listPatients = new List<Patient>();
            listDailyGoals = new List<DailyGoal>();
            listWeeklyGoals = new List<WeeklyGoal>();
            listMealPLans = new List<MealPlan>();

            GetPatientIdData();
            //GetDataPrueba();
            //GetPatientData();
        }

       
        private async void GetPatientIdData()
        {
            listPatients = (List<Patient>)await PatientService.Instance.GetAllPatients();

            Patient pa = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            //prueba1.Text = listPatients.Last().Id.ToString();
            //prueba2.Text = listPatients.Last().ProfileId.ToString();

            foreach(Patient patient in listPatients)
            {
                if (patient.ProfileId.ToString() == _profile.Id.ToString())
                {
                    prueba1.Text=_patientId.Id.ToString();
                    prueba2.Text = patient.Id.ToString();
                    _patientId = patient;
                    //prueba3.Text = _patientId.Id.ToString();
                    //prueba4.Text = patient.Id.ToString();
                }
            }

        }

        private async void DeleteUser_Clicked(object sender, EventArgs e)
        {

            //primero eliminar daily, weekly, meals
            patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            prueba3.Text = _patientId.Id.ToString();

            if(_profile.RoleId == 1)
            {
                listDailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(patient.Id);
                listWeeklyGoals = await WeeklyGoalService.Instance.GetAllWeeklyGoalsByPatientId(patient.Id);
                listMealPLans = await MealPlanService.Instance.GetMealPlansByPatientId(patient.Id);

                foreach (DailyGoal dailyGoal in listDailyGoals)
                {
                    if(listDailyGoals != null)
                    {
                        await DailyGoalService.Instance.DeleteDailyGoalByPatientId(_patientId.Id, dailyGoal);
                    }
                }

                foreach (WeeklyGoal weeklyGoal in listWeeklyGoals)
                {
                    if(listWeeklyGoals != null)
                    {
                        await WeeklyGoalService.Instance.DeleteWeeklyGoalByPatientId(_patientId.Id, weeklyGoal);
                    }
                }

                foreach (MealPlan mealPlan in listMealPLans)
                {
                    if(listMealPLans != null)
                    {
                        await MealPlanService.Instance.DeleteMealPlanByPatientId(_patientId.Id, mealPlan);
                    }
                }

                //await PatientService.Instance.DeletePatient(patient.Id);

            }



            //segundo eliminar paciente
            //tercero eliminar profile
            //cuarto eliminar user

            await Navigation.PushAsync(new AdminHomePage());

        }
        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }
    }
}