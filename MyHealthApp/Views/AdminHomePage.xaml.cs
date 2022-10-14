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
        public static SlProfileDetailsViewModel Patient;
        public static List<Patient> _patient;
        public static PatientsProfilesViewModel _viewModel;
        public static List<Specialist> Specialists;


        public static List<Profile> Profiles;
        public static ProfileViewModel profileView;

        public long id;
        public static UserViewModel userView;

        public long roleIDProfile;
        public static Profile profileUser;
        //public UserViewModel userView;

        public AdminHomePage()
        {
            InitializeComponent();
            //Users = new List<User>();
            Profiles = new List<Profile>();
            profileUser = new Profile();
            //_viewModel = new PatientsProfilesViewModel(_patient);
            Device.BeginInvokeOnMainThread(() =>
            {

                GetData();
            });
            //GetData();

        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void GetData()
        {

            for ( int i = 0; i < 100; i++)
            {
                profileUser = await ProfileService.Instance.GetProfileById(i);
                if (profileUser != null)
                {
                    if(profileUser.RoleId != 3)
                    {
                        Profiles.Add(profileUser);
                    }
                }
            }
            profileView = new ProfileViewModel(Profiles);


            StackLayoutPatients.BindingContext = profileView;
        }

        private async void FrameProfile_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var profile = param as Profile;

                if (profile != null)
                {
                    //BUG: El progress ring no renderiza del data template con data asíncrona
                    //Solucion parcial es esta
                    var patient = await PatientService.Instance.GetPatientByProfileId(profile.Id);
                    var profileUser = await ProfileService.Instance.GetProfileById(profile.Id);

                    await Navigation.PushAsync(new EditProfileAsAdminPage(profileUser));
                }
            }
        }
    }   
}