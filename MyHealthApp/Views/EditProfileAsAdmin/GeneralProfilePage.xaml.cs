using System;
using System.Net;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Services.Activities;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.DeletePatient;
using MyHealthApp.Views.EditProfileAsAdmin.DeleteProfileMD;
using MyHealthApp.Views.ProfileFlow;
using Xamarin.CommunityToolkit.Extensions;
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
            var result = await Navigation.ShowPopupAsync(new DeleteProfileConfirmation(_profile, _patient, _specialist));
            if (result != null && (int)result == 2)
            {
                //await Navigation.PopAsync();
            }
            

        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
	        await Navigation.PopAsync();
        }

        
	}
}