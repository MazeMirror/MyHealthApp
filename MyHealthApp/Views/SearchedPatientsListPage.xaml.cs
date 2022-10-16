using System;
using System.Collections.Generic;
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
    public partial class SearchedPatientsListPage : ContentPage
    {
        public  PatientsProfilesViewModel _viewModel;
        public Specialist Specialist;
        public SearchedPatientsListPage(Specialist specialist)
        {
            InitializeComponent();
            this.Specialist = specialist;
            _viewModel = new PatientsProfilesViewModel();
            
            GetListOfPatientsProfiles();
            //GetListOfPatientsProfiles();
            
            BindingContext = _viewModel;
        }

        private async void GetListOfPatientsProfiles()
        {
            var totalProfiles = await ProfileService.Instance.GetProfilesByRoleId(1);
            foreach (var itemProfile in totalProfiles)
            {
                _viewModel.AddProfileToList(itemProfile);
            }
        }

        /*private async void GetListOfPatientsProfiles()
        {
            var totalPatients = await PatientService.Instance.GetAllPatients();

            foreach (var item in totalPatients)
            {
                var patientProfile = await ProfileService.Instance.GetProfileById(item.ProfileId);
                _viewModel.Profiles.Add(patientProfile);
                //Profiles.Add(patientProfile);
            }
            
            
            var patient1 = new Profile() {Name = "Josias", LastName = "Olaya",BirthDate = DateTime.Now};
            this._viewModel.Profiles.Add(patient1);
            this._viewModel.Profiles.Add(patient1);
            this._viewModel.Profiles.Add(patient1);
            this._viewModel.Profiles.Add(patient1);
        }*/

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ButtonAddPatient_OnClick(object sender, EventArgs e)
        {
            var item = sender as Button;
            if (item != null)
            {
                item.IsEnabled = false;
                var currentIdItem = item.CommandParameter.ToString();
                var profileId = int.Parse(currentIdItem);
                var profile = await ProfileService.Instance.GetProfileById(profileId);

                if (profile == null) return;

                var patient = await PatientService.Instance.GetPatientByProfileId(profileId);



                var isPresentInList = PatientsListPage._viewModel.ItemAlreadyExist(profile);

                if (isPresentInList)
                {
                    await DisplayAlert("Este paciente ya se encuentra en tu lista", "Selecciona otro", "Ok");
                    item.IsEnabled = true;
                }
                else
                {
                    var response = await SpecialistService.Instance.AssignSpecialistWitPatient(this.Specialist.Id, patient.Id);

                    if (!response)
                    {
                        await DisplayAlert("Error", "No se pudo asignar el paciente al especialista", "Ok");
                        item.IsEnabled = true;
                        return;
                    }
                    
                    PatientsListPage._viewModel.AddProfileToList(profile);
                    SpecialistHomePage.ViewModel.AddProfileToList(profile);
                    await DisplayAlert("Añadido exitosamente", "El paciente aparecerá ahora en tu lista", "Ok");
                    item.IsEnabled = true;
                    await Navigation.PopAsync();
                }

               
                
                //await DisplayAlert("Alert", currentItem, "Ok");
            }
        }

        private async void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            string roleId = "1"; //Osea de rol paciente 
            var searchedPatients = await ProfileService.Instance.GetProfileByNameAndRoleId(SearchBar.Text,roleId);
            
            _viewModel.ClearProfileList();
            
            foreach (var item in searchedPatients)
            {
                _viewModel.AddProfileToList(item);
            }
        }

        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string roleId = "1"; //Osea de rol paciente 
            var searchedPatients = await ProfileService.Instance.GetProfileByNameAndRoleId(SearchBar.Text,roleId);
            _viewModel.ClearProfileList();
            
            foreach (var item in searchedPatients)
            {
                _viewModel.AddProfileToList(item);
            }
        }
    }
}