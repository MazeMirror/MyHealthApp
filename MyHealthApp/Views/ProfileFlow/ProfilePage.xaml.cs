using System;
using System.Collections.Generic;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
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
        public Specialist specialist;
        public Patient patient;
        public User user;
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
            var userEntity = await App.SqLiteDb.GetUserAsync();
            user = ConvertToModel.ConvertToUserModel(userEntity);
            
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
            

            if (LocalProfile.RoleId == 1)
            {
                patient = await PatientService.Instance.GetPatientByProfileId(LocalProfile.Id);
                
                Model = new SlProfileDetailsViewModel(LocalProfile, patient, userEntity);
                BindingContext = Model;
            }
            else
            {
                //Hacemos peticion al backend para obtener el especialista por RoleId
                
                /*var specialist = new Specialist()
                {
                    Specialty = "Pediatria",
                };*/
                specialist = await SpecialistService.Instance.GetSpecialistByProfileId(LocalProfile.Id);
                
                Model = new SlProfileDetailsViewModel(LocalProfile, specialist, userEntity);
                BindingContext = Model;
            }
        }
        

        private async void LabelEditName_OnTapped(object sender, EventArgs e)
        {
            /*var a = new EditProfileNamesPage();
            var tabbeChildren = a as TabbedPage;
            await Navigation.PushAsync(new EditProfileNamesPage());*/
            //NOTA PERSONAL: Los tabbed page no soportan lanzar hijos con NavigationPage PushAsync
            //Si fueron los tabbed page envueltos en ModalPage
            //Recordar poner <Navigation></> para las pages internas de 
            //las tabbed en XAML que requieran navegarse
            //await Application.Current.MainPage.Navigation.PushAsync(new EditProfileNamesPage());
            await Navigation.PushAsync(new EditProfileNamesPage());
            //Este es el truco
        }

        private async void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {
            if (LocalProfile.RoleId == 1) await Navigation.PushAsync(new EPPatientDetailsPage());
            else await Navigation.PushAsync(new EPSpecialistDetailsPage(LocalProfile,specialist,user));
            //await Navigation.PushAsync(new EditProfileDetailsPage() );
        }

        private async void LabelLogout_OnTapped(object sender, EventArgs e)
        {
            await App.SqLiteDb.DeleteAllProfileAsync();
            await App.SqLiteDb.DeleteAllUsersAsync();
            Application.Current.Properties["RoleLogged"] = 3;
            Application.Current.MainPage = new NavigationPage(new WelcomePage());
            //await Navigation.PopToRootAsync();
        }
    }
}