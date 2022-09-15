using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
            //Hacemos la peticion al backend para autenticar
            //Y recibimos el perfil asociado al user
            
            //Si tenemos exito en ello, guardamos en SQLlite los datos de perfil (PerfilId,UserId y datos especificos) 
            
            
            //Dependiendo del rolID de perfil mandamos el Tabbed de patient O Specialist
            await Navigation.PushModalAsync(new TabbedPatient());
        }
    }
}