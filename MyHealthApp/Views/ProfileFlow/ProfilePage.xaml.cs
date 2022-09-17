using System;
using System.Collections.Generic;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public static SlProfileDetailsViewModel Model;
        public static Profile LocalProfile;
        public ProfilePage()
        {
            InitializeComponent();
            
            GetDataInformation();

        }

        private async void GetDataInformation()
        {
            //Hacemos peticion a SQLlite para obtener el perfil y el user
            
            //ilustrativo
            /*Random rnd = new Random();
            int num = rnd.Next(2);*/
            /*var profile = new Profile()
            {
                Gender = "Masculino",
                Name = "Josias Josue",
                LastName = "Olaya Chauca",
                ImageUrl = "https://....",
                BirthDate = DateTime.Now,
                RoleId = 0, 
            };
            
            

            var user = new User()
            {
                Email = "JosiasOlaya@hotmail.com"
            };*/
            ////////////////////////////////////////////////////////
            var user = await App.SqLiteDb.GetUserAsync();
            var profileEntity = await App.SqLiteDb.GetProfileAsync();

            LocalProfile = new Profile()
            {
                Id = profileEntity.Id,
                Name = profileEntity.Name,
                LastName = profileEntity.LastName,
                Gender = profileEntity.Gender,
                BirthDate = profileEntity.BirthDate,
                ImageUrl = profileEntity.ImageUrl,
                RoleId = profileEntity.RoleId,
                UserId = profileEntity.UserId
            };

            LabelName.BindingContext = LocalProfile;
            LabelName.SetBinding(Label.TextProperty, new Binding() { Path = "Name" });

            LabelLastname.BindingContext = LocalProfile;
            LabelLastname.SetBinding(Label.TextProperty, new Binding(){ Path = "LastName"});
            

            if (LocalProfile.RoleId == 0)
            {
                var patient = await PatientService.Instance.GetPatientByProfileId(LocalProfile.Id);
                
                Model = new SlProfileDetailsViewModel(LocalProfile, patient, user);
                BindingContext = Model;
            }
            else
            {
                //Hacemos peticion al backend para obtener el especialista por RoleId
                
                /*var specialist = new Specialist()
                {
                    Specialty = "Pediatria",
                };*/
                var specialist = await SpecialistService.Instance.GetSpecialistByProfileId(LocalProfile.Id);
                
                Model = new SlProfileDetailsViewModel(LocalProfile, specialist, user);
                BindingContext = Model;
            }
        }
        

        private async void LabelEditName_OnTapped(object sender, EventArgs e)
        {
            /*var a = new EditProfileNamesPage();
            var tabbeChildren = a as TabbedPage;
            await Navigation.PushAsync(new EditProfileNamesPage());*/
            //NOTA PERSONAL: Los tabbed page no soportan lanzar hijos con NavigationPage PushAsync 
            //await Application.Current.MainPage.Navigation.PushAsync(new EditProfileNamesPage());
            await Navigation.PushModalAsync(new EditProfileNamesPage() );
            //Este es el truco
        }

        private void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {
            
        }

        private void LabelDeleteAccount_OnTapped(object sender, EventArgs e)
        {
            
        }
    }
}