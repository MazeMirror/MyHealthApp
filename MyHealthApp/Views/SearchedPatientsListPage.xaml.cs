using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.SuccesfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchedPatientsListPage : Popup
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
                    Navigation.ShowPopup(new SMPage("Este paciente ya se encuentra en tu lista", "Selecciona otro", false, false));
                    //await DisplayAlert("Este paciente ya se encuentra en tu lista", "Selecciona otro", "Ok");
                    item.IsEnabled = true;
                }
                else
                {
                    var response = await SpecialistService.Instance.AssignSpecialistWitPatient(this.Specialist.Id, patient.Id);

                    if (!response)
                    {
                        Navigation.ShowPopup(new SMPage("Error", "No se pudo asignar el paciente al especialista", false, false));
                        //await DisplayAlert("Error", "No se pudo asignar el paciente al especialista", "Ok");
                        item.IsEnabled = true;
                        return;
                    }
                    
                    PatientsListPage._viewModel.AddProfileToList(profile);
                    SpecialistHomePage.ViewModel.AddProfileToList(profile);
                    Navigation.ShowPopup(new SMPage("Añadido exitosamente", "El paciente aparecerá ahora en tu lista", false, false));
                    //await DisplayAlert("Añadido exitosamente", "El paciente aparecerá ahora en tu lista", "Ok");
                    item.IsEnabled = true;
                    Dismiss(2);
                    //await Navigation.PopAsync();
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