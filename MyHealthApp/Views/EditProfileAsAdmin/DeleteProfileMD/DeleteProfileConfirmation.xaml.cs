using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Services.Activities;
using MyHealthApp.ViewModels;
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

namespace MyHealthApp.Views.EditProfileAsAdmin.DeleteProfileMD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteProfileConfirmation : Popup
    {
        private Profile _profile;
        private Patient _patient;
        private Specialist _specialist;
        public static SlProfileDetailsViewModel Model;
        public DeleteProfileConfirmation(Profile profile, Patient patient, Specialist specialist)
        {
            _profile = profile;
            _patient = patient;
            _specialist = specialist;
            InitializeComponent();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void DeleteProfileButton_Clicked(object sender, EventArgs e)
        {
            if (_profile.RoleId == 1)
            {
                DeletePatientAccount();
                //Dismiss(2);
            }
            else
            {
                DeleteSpecialistAccount();
                //Dismiss(2);
            }

           // await Navigation.PushAsync(new AdminHomePage());
        }

        private async void DeletePatientAccount()
        {
            //

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                var mealPlans = await MealPlanService.Instance.GetMealPlansByPatientId(_patient.Id);
                var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(_patient.Id);
                var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(_patient.Id);

                var stepRecord = await
                    StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patient.Id, DateTime.MinValue, DateTime.MaxValue);
                var kilocalorieRecord = await
                    KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patient.Id, DateTime.MinValue, DateTime.MaxValue);
                var distanceRecord = await
                    DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patient.Id, DateTime.MinValue, DateTime.MaxValue);

                //Eliminamos su relación de 1 a muchos

                foreach (var mealPlan in mealPlans)
                {
                    await MealPlanService.Instance.DeleteMealPlanByPatientId(_patient.Id, mealPlan);
                }

                foreach (var dg in dailyGoals)
                {
                    await DailyGoalService.Instance.DeleteDailyGoalByPatientId(_patient.Id, dg);
                }

                foreach (var wg in weeklyGoals)
                {
                    await WeeklyGoalService.Instance.DeleteWeeklyGoalByPatientId(_patient.Id, wg);
                }

                foreach (var stepRd in stepRecord)
                {
                    await StepService.Instance.DeleteStepActivityByPatientId(_patient.Id, stepRd);
                }

                foreach (var distanceRd in distanceRecord)
                {
                    await DistanceService.Instance.DeleteDistanceActivityByPatientId(_patient.Id, distanceRd);
                }

                foreach (var kilocalorieRd in kilocalorieRecord)
                {
                    await KilocalorieService.Instance.DeleteKilocalorieActivityByPatientId(_patient.Id, kilocalorieRd);
                }


                //Ahora desasignamos el paciente de sus especialistas
                var specialistList = await PatientService.Instance.GetSpecialistByPatientId(_patient.Id);
                foreach (var specialist in specialistList)
                {
                    await SpecialistService.Instance.UnassignSpecialistWitPatient(specialist.Id, _patient.Id);
                }

                //Ahora eliminamos el paciente
                var resp1 = await PatientService.Instance.DeletePatient(_patient.Id);

                var profile = await ProfileService.Instance.GetProfileById(_patient.ProfileId);

                //ahora eliminamos el perfil
                var resp2 = await ProfileService.Instance.DeleteProfileById(profile.Id);


                //ahora eliminamos el usuario
                var resp3 = await UserService.Instance.DeleteUserById(profile.UserId);

                if (resp3 == HttpStatusCode.OK)
                {
                    //Eliminar de la lista si amerita
                    
                    
                    Navigation.ShowPopup(new DeleteProfileSuccess());
                }
            });


        }

        private async void DeleteSpecialistAccount()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {

                //Primero borramos sus asignaciones a pacientes
                var patients = await SpecialistService.Instance.GetPatientsBySpecialistId(_specialist.Id);

                foreach (var item in patients)
                {
                    await SpecialistService.Instance.UnassignSpecialistWitPatient(_specialist.Id, item.Id);
                }

                //Ahora eliminamos el especialista
                var resp1 = await SpecialistService.Instance.DeleteSpecialistById(_specialist.Id);

                var profile = await ProfileService.Instance.GetProfileById(_specialist.ProfileId);

                //ahora eliminamos el perfil
                var resp2 = await ProfileService.Instance.DeleteProfileById(profile.Id);


                //ahora eliminamos el usuario
                var resp3 = await UserService.Instance.DeleteUserById(profile.UserId);

                if (resp3 == HttpStatusCode.OK)
                {
                    //Eliminar de la lista si amerita
                    //...
                    
                    
                    Navigation.ShowPopup(new DeleteProfileSuccess());
                }

            });
        }
    }
}