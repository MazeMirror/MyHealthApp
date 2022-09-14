using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterCredentialPage : ContentPage
    {
        private Profile _profile;
        private User _user;

        
        public RegisterCredentialPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            _user = new User();
            
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(EntryFirstPassword.Text) || 
                string.IsNullOrWhiteSpace(EntrySecondPassword.Text) ||
                string.IsNullOrWhiteSpace(EntryEmail.Text)
                )
            {
                await DisplayAlert("Advertencia",
                    "Hay presencia de campos vacíos, por favor complételos antes de continuar", "Ok");
                return;
            }
            
            if (EntryFirstPassword.Text != EntrySecondPassword.Text)
            {
                await DisplayAlert("Advertencia",
                    "Las contraseñas no son iguales", "Ok");
                return;
            }
            
            _user.Email = EntryEmail.Text;
            _user.Password = EntryFirstPassword.Text;
            
            //Creamos el usuario
            long userId = await UserService.Instance.PostUser(_user);
            
            if (userId == -1)
            {
                await DisplayAlert("Error", "Ocurrió un error al registrar el usuario", "Ok");
            }
            else
            {
                //Creamos el perfil
                _profile.UserId = userId;
                long profileId = await ProfileService.Instance.PostProfile(_profile);
                if (profileId == -1)
                {
                    await DisplayAlert("Error", "Ocurrio un error al registrar el perfil", "Ok");
                }
                else
                {
                    //Creamos un especialista o un paciente

                    if (_profile.RoleId == 0)
                    {
                        var patient = new Patient() { ProfileId = profileId };
                        await PatientService.Instance.PostPatient(patient);
                    }
                    else
                    {
                        var specialist = new Specialist() { ProfileId = profileId,Specialty = ""};
                        await SpecialistService.Instance.PostSpecialist(specialist);
                    }
                    
                    await Navigation.PushModalAsync(new SuccessfulRegisterPage());
                }
            }
            
            
            
        }
        
        
        
        
        
        

        private void ButtonShowSecondPassword_OnClicked(object sender, EventArgs e)
        {
            this.EntrySecondPassword.IsPassword = !this.EntrySecondPassword.IsPassword;
            this.ButtonShowSecondPassword.Text = this.EntrySecondPassword.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();
        }

        private void ButtonShowFirstPassword_OnClicked(object sender, EventArgs e)
        {
            this.EntryFirstPassword.IsPassword = !this.EntryFirstPassword.IsPassword;
            this.ButtonShowFirstPassword.Text = this.EntryFirstPassword.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();

        }
    }
}