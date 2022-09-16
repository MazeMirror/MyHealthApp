using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Views.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void PasswordButton_OnClicked(object sender, EventArgs e)
        {
            this.EntryPassword.IsPassword = !this.EntryPassword.IsPassword;
            this.PasswordButton.Text = this.EntryPassword.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();
        }

        private async void RegisterLabel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectAccountTypePage());
        }

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text) ||
                string.IsNullOrWhiteSpace(EntryPassword.Text))
            {
                await DisplayAlert("Advertencia",
                    "Hay presencia de campos vacíos, por favor complételos antes de continuar", "Ok");
                return;
            }
            
            /*

            //Hacemos la peticion al backend para autenticar
            //Y recibimos el perfil asociado al user
            User user = await UserService.Instance.PostAuthenticateUser(EntryEmail.Text, EntryPassword.Text);
            if (user == null)
            {
                await DisplayAlert("Credenciales invalidos", "Revisar", "Ok");
                return;
            }

            Profile profile = await ProfileService.Instance.GetProfileByUserId(user.Id);
            if (profile == null) return;
            
            //Si tenemos exito en ello, guardamos en SQLlite los datos de perfil (PerfilId, UserCorreo y datos de perfil) 
            //2 entidades: Profile y User (sin contraseña obviamente)
            await App.SqLiteDb.SaveProfileAsync(new ProfileEntity()
            {
                Id = profile.Id,
                Gender = profile.Gender,
                Name = profile.Name,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate,
                ImageUrl = profile.ImageUrl,
                RoleId = profile.RoleId,
                UserId = profile.UserId
            });

            await App.SqLiteDb.SaveUserAsync(new UserEntity()
            {
                Id = user.Id,
                Email = user.Email
            });
            */
            
            
            
            //Dependiendo del rolID de perfil mandamos el Tabbed de patient O Specialist
            if(true ) await Navigation.PushModalAsync(new TabbedPatient());
            else await Navigation.PushModalAsync(new TabbedSpecialist());
        }
    }
}