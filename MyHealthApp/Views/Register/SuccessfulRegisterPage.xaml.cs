using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulRegisterPage : ContentPage
    {
        private readonly User _user;
        private readonly Profile _profile;
        public SuccessfulRegisterPage(User user, Profile profile)
        {
            InitializeComponent();
            LabelTitle.Text = $"Hola, {profile.Name}\n bienvenido a";
            if (profile.RoleId == 1)
            {
                LabelWelcomeMessage.Text =
                    "Su cuenta ha sido creada con éxito. \nEs hora de alcanzar todas esas \nmetas y mejorar el rendimiento físico";
            }
            else
            {
                LabelWelcomeMessage.Text = "Su cuenta ha sido creada con éxito. \nEs hora de ayudar a las personas \na que mejoren su rendimiento físico";
            }
            
            _user = user;
            _profile = profile;
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            //Efectuamos el guardado en SQLlite
            //user y profile
            //
            //User user = await UserService.Instance.PostAuthenticateUser(_user.Email, _user.Password);
            //Profile profile = await ProfileService.Instance.GetProfileByUserId(user.Id);

            await App.SqLiteDb.SaveProfileAsync(new ProfileEntity()
            {
                Id = _profile.Id,
                Gender = _profile.Gender,
                Name = _profile.Name,
                LastName = _profile.LastName,
                BirthDate = _profile.BirthDate,
                ImageUrl = _profile.ImageUrl,
                RoleId = _profile.RoleId,
                UserId = _profile.UserId
            });

            await App.SqLiteDb.SaveUserAsync(new UserEntity()
            {
                Id = _user.Id,
                Email = _user.Email
            });

            if (_profile.RoleId == 1)
            {
                var patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
                Application.Current.Properties["PatientId"] = patient.Id.ToString();
                Application.Current.MainPage = new NavigationPage(new TabbedPatient());
            }
            else Application.Current.MainPage = new NavigationPage(new TabbedSpecialist());
            
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}