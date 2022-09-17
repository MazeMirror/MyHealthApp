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
        private PatientsProfilesViewModel _viewModel;

        public PatientsListPage()
        {
            
            
            InitializeComponent();
            _viewModel = new PatientsProfilesViewModel();
            GetListOfPatientsProfiles();
            
            BindingContext = _viewModel;
        }

        private void GetListOfPatientsProfiles()
        {
            /*var specialistProfile = await App.SqLiteDb.GetProfileAsync();
            var specialist = await SpecialistService.Instance.GetSpecialistByProfileId(specialistProfile.Id);
            
            var localPatients = await SpecialistService.Instance.GetPatientsBySpecialistId(specialist.Id);

            foreach (var item in localPatients)
            {
                var patientProfile = await ProfileService.Instance.GetProfileById(item.ProfileId);
                this.viewModel.Profiles.Add(patientProfile);
                //Profiles.Add(patientProfile);
            }*/
            
            _viewModel.Profiles.Add(new Profile(){Name = "Jose",LastName = "Uru"});
            _viewModel.Profiles.Add(new Profile(){Name = "Josias",LastName = "Mura"});
            _viewModel.Profiles.Add(new Profile(){Name = "Maycol",LastName = "Amigg"});
            _viewModel.Profiles.Add(new Profile(){Name = "Carlos",LastName = "Holaa"});
            _viewModel.Profiles.Add(new Profile(){Name = "Juan",LastName = "NONO"});
            
        }

        private void LabelAddPatient_OnTapped(object sender, EventArgs e)
        {
            
        }
    }
}