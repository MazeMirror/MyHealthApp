using System;
using System.Net;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Services.Activities;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GeneralProfilePage : ContentPage
	{
		private Profile _profile;
		private Patient _patient;

		private Specialist _specialist;
		public static SlProfileDetailsViewModel Model;
		
		public GeneralProfilePage (Profile profile)
		{
			InitializeComponent ();
			_profile = profile;
			GetDataInformation();
		}
		
		private async void GetDataInformation()
        {
	        
            LabelName.BindingContext = _profile;
            LabelName.SetBinding(Label.TextProperty, new Binding() { Path = "Name" });

            LabelLastname.BindingContext = _profile;
            LabelLastname.SetBinding(Label.TextProperty, new Binding(){ Path = "LastName"});

            var user = await UserService.Instance.GetUserById(_profile.UserId);

            var userEntity = ConvertToEntity.ConvertToUserEntity(user);

            if (_profile.RoleId == 1)
            {
                _patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
                
                
                Model = new SlProfileDetailsViewModel(_profile, _patient, userEntity);
                BindingContext = Model;
            }
            else
            {
                
                _specialist = await SpecialistService.Instance.GetSpecialistByProfileId(_profile.Id);
                
                Model = new SlProfileDetailsViewModel(_profile, _specialist, userEntity);
                BindingContext = Model;
            }
        }

		
		
        private async void LabelEditName_OnTapped(object sender, EventArgs e)
        {
	        await Navigation.PushAsync(new EditProfileNamesAsAdminPage(_profile));
        }

        private async void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {
	        if (_profile.RoleId == 1) await Navigation.PushAsync(new EPPatientAsAdminPage(_profile));
	        else await Navigation.PushAsync(new EPSpecialistAsAdminPage(_profile));
        }
        

        private async void DeleteAccount_OnTapped(object sender, EventArgs e)
        {
	        if (_profile.RoleId == 1)
	        {
		        DeletePatientAccount();
	        }
	        else
	        {
		        DeleteSpecialistAccount();
	        }
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
	        await Navigation.PopAsync();
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
			        StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patient.Id,DateTime.MinValue,DateTime.MaxValue);
		        var kilocalorieRecord = await 
			        KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patient.Id,DateTime.MinValue,DateTime.MaxValue);
		        var distanceRecord = await 
			        DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patient.Id,DateTime.MinValue,DateTime.MaxValue);

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
			        await StepService.Instance.DeleteStepActivityByPatientId(_patient.Id,stepRd);
		        }
		        
		        foreach (var distanceRd in distanceRecord)
		        {
			        await DistanceService.Instance.DeleteDistanceActivityByPatientId(_patient.Id,distanceRd);
		        }

		        foreach (var kilocalorieRd in kilocalorieRecord)
		        {
			        await KilocalorieService.Instance.DeleteKilocalorieActivityByPatientId(_patient.Id,kilocalorieRd);
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
                    //...
                    await Navigation.PushAsync(new AdminHomePage());
                }
                else
		        {
			        await DisplayAlert("Importante", "No se pudo eliminar el paciente y su usuario", "Ok");
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
					await Navigation.PushAsync(new AdminHomePage());
				}
				else
				{
					await DisplayAlert("Importante", "No se pudo eliminar el especialista y su usuario", "Ok");
				}

	        });
        }
	}
}