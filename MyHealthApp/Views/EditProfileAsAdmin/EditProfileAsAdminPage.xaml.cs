using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditProfileAsAdminPage : ContentPage
	{
		private Profile _profile;
		private String Specialty;
		private String Email;
		public EditProfileAsAdminPage (Profile profile)
		{
			InitializeComponent ();
			_profile = profile;
			GetDataProfile();
        }

		public async void GetDataProfile()
		{
            LabelName.Text = _profile.Name;
            LabelLastname.Text = _profile.LastName;

			if(_profile.RoleId == 1)
			{
				Specialty = "Paciente";
			} else
			{
				Specialty = "Nutricionista";
			}

            var user = await UserService.Instance.GetUserById(_profile.Id);
            Email = user.Email;

            LabelEditDetails.Text = "Genero: " + _profile.Gender + "\n\nEdad: " + _profile.Years +
                "\n\nFecha de Nacimiento: " + _profile.BirthDate + "\n\nEspecialidad: " + Specialty +
                "\n\nEmail: " + Email;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

		private async void LabelLogout_OnTapped(object sender, EventArgs e)
		{

		}
        private async void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {

        }
    }
}