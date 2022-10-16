using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Views.ProfileFlow;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfileNamesAsAdminPage : ContentPage
    {
        private Profile _profile;
        public EditProfileNamesAsAdminPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            
            EntryName.Text = _profile.Name;
            EntryLastName.Text = _profile.LastName;
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_OnClicked(object sender, EventArgs e)
        {
            ButtonSaveChanges.IsEnabled = false;
            
            if (string.IsNullOrWhiteSpace(EntryName.Text) ||
                string.IsNullOrWhiteSpace(EntryLastName.Text))
            {
                ButtonSaveChanges.IsEnabled = true;
                await DisplayAlert("Advertencia",
                    "Hay presencia de campos vacíos, por favor complételos antes de continuar", "Ok");
                return;
            }

            var profileAux = _profile.CreateDeepCopy();

            profileAux.Name = EntryName.Text;
            profileAux.LastName = EntryLastName.Text;
            
            var updatedProfile = await ProfileService.Instance.PutProfileByProfileAndId(profileAux,_profile.Id);
            if (updatedProfile == null)
            {
                ButtonSaveChanges.IsEnabled = true;
                await DisplayAlert("Error", "No se pudo actualizar el perfil, vuelva a intentarlo", "Ok");
            }
            else
            {
                _profile.Name = EntryName.Text;
                _profile.LastName = EntryLastName.Text;
                
                await Navigation.PopAsync();
            }
            

        }
    }
}