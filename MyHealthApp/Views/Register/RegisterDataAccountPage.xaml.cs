using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Xamarin.CommunityToolkit;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterDataAccountPage : ContentPage
    {
        private Profile _profile;
        public RegisterDataAccountPage(Role role)
        {
            InitializeComponent();
            _profile = new Profile
            {
                RoleId = role.Id
            };
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryLastname.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
            {
                await DisplayAlert("Advertencia",
                    "Hay presencia de campos vacíos, por favor complételos antes de continuar", "Ok");
                return;
            }
            _profile.Name = EntryName.Text;
            _profile.LastName = EntryLastname.Text;
            _profile.BirthDate = DatePickerBirthdate.Date;
            _profile.Gender = PickerGenre.SelectedItem.ToString();
            //aun no tengo userId
            
            await Navigation.PushAsync(new RegisterCredentialPage(_profile));
        }
    }
}