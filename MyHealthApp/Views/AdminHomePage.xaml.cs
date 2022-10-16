using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.EditPatientGoal;
using MyHealthApp.Views.EditProfileAsAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminHomePage : ContentPage
    {
        public static List<Profile> Profiles;
        public static ProfileViewModel profileView;
        public static Profile profileUser;
        public long TotalAccounts;
        public PatientsProfilesViewModel _viewModel;
        public AdminHomePage()
        {
            InitializeComponent();
            Profiles = new List<Profile>();
            profileUser = new Profile();
            _viewModel = new PatientsProfilesViewModel();

            GetListOfPatientsProfiles();

            BindingContext = _viewModel;
        }

        private async void GetListOfPatientsProfiles()
        {
            var totalProfiles = await ProfileService.Instance.GetProfilesByRoleId(1);
            var totalProfiles2 = await ProfileService.Instance.GetProfilesByRoleId(2);

            foreach (var itemProfile in totalProfiles)
            {
                _viewModel.AddProfileToList(itemProfile);
            }

            foreach (var itemProfile2 in totalProfiles2)
            {
                _viewModel.AddProfileToList(itemProfile2);
            }
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new WelcomePage());
        }

        private async void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            string roleId = "1"; //Osea de rol paciente 
            var searchedPatients = await ProfileService.Instance.GetProfileByNameAndRoleId(SearchBar.Text, roleId);

            _viewModel.ClearProfileList();

            foreach (var item in searchedPatients)
            {
                _viewModel.AddProfileToList(item);
            }
        }

        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            string roleId = "1"; //Osea de rol paciente 
            var searchedPatients = await ProfileService.Instance.GetProfileByNameAndRoleId(SearchBar.Text, roleId);
            _viewModel.ClearProfileList();

            foreach (var item in searchedPatients)
            {
                _viewModel.AddProfileToList(item);
            }
        }

        private async void FrameProfile_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var profile = param as Profile;

                if (profile != null)
                {
                    await Navigation.PushAsync(new GeneralProfilePage(profile));
                }
            }
        }
    }   
}