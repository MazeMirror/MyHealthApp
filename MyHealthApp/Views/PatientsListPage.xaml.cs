using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientsListPage : ContentPage
    {
        public static PatientsProfilesViewModel _viewModel;
        public Specialist Specialist;
        

        public PatientsListPage()
        {
            
            InitializeComponent();
            _viewModel = new PatientsProfilesViewModel();
            GetListOfMyPatientsProfiles();
            
            BindingContext = _viewModel;
        }

        private async void GetListOfMyPatientsProfiles()
        {
            var specialistProfile = await App.SqLiteDb.GetProfileAsync(); 
            Specialist = await SpecialistService.Instance.GetSpecialistByProfileId(specialistProfile.Id);
            
            var localPatients = await SpecialistService.Instance.GetPatientsBySpecialistId(Specialist.Id);

            foreach (var item in localPatients)
            {
                var patientProfile = await ProfileService.Instance.GetProfileById(item.ProfileId);
                _viewModel.Profiles.Add(patientProfile);
                //Profiles.Add(patientProfile);
            }

            /*if (_viewModel.Profiles.Count == 0)
            {
                 Label = new Label
                {
                    Text = "Usted todavía no cuenta con pacientes. \nAñada nuevos a la lista",
                    TextColor = Color.FromHex("#6EB3CD"),
                    FontSize = 16,
                    FontFamily = "ArchivoRegular",
                    HorizontalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(0,20,0,0)
                };
                
                StackLayoutPatients.Children.Add(Label);
            }*/
            
            /*_viewModel.Profiles.Add(new Profile(){Name = "Jose",LastName = "Uru"});
            _viewModel.Profiles.Add(new Profile(){Name = "Josias",LastName = "Mura"});
            _viewModel.Profiles.Add(new Profile(){Name = "Maycol",LastName = "Amigg"});
            _viewModel.Profiles.Add(new Profile(){Name = "Carlos",LastName = "Holaa"});
            _viewModel.Profiles.Add(new Profile(){Name = "Juan",LastName = "NONO"});*/
            
        }

        private async void LabelAddPatient_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchedPatientsListPage(Specialist));
        }

        private async void FramePatient_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var profile = param as Profile;
                //var currentIdItem = param.ToString();
                //var profileId = int.Parse(currentIdItem);
                //var patient = await PatientService.Instance.GetPatientByProfileId(profileId);
                await Navigation.PushAsync(new PatientDetailsPage(profile));

                //await DisplayAlert("Hola", "El Id es " + profileId.ToString(), "Ok");
            }
        }

        private void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            
        }
    }
}