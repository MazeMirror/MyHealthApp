using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientDetailsPage : ContentPage
    {
        private Profile _profile;
        private SlProfileDetailsViewModel _model;

        public PatientDetailsPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile.CreateDeepCopy();
            //LabelName.Text = _profile.Name;
            //LabelLastname.Text = _profile.LastName;
            GetDataInformation();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void GetDataInformation()
        {
            var patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
            var user = await UserService.Instance.GetUserById(_profile.UserId);
            _model = new SlProfileDetailsViewModel(_profile, patient, ConvertToEntity.ConvertToUserEntity(user));
            BindingContext = _model;
        }
    }
}