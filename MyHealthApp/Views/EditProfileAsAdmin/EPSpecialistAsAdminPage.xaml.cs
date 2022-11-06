using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Views.ProfileFlow;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EPSpecialistAsAdminPage : ContentPage
    {
        private Profile _profile;
        private Specialist _specialist;
        private User _user;
        public EPSpecialistAsAdminPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            CompleteFields();
        }
        
        private async void CompleteFields()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                _specialist = await SpecialistService.Instance.GetSpecialistByProfileId(_profile.Id);
                _user = await UserService.Instance.GetUserById(_profile.UserId);
                
                DatePickerBirthdate.Date = _profile.BirthDate.AddDays(1);
                PickerGenre.SelectedItem = _profile.Gender;
                EntryEspeciality.Text = _specialist.Specialty;
                EntryEmail.Text = _user.Email;
                EntryEmail.Text = _user.Email;
            });
            
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEspeciality.Text) ||
                string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Advertencia",
                    "Hay presencia de campos vacíos, por favor complételos antes de continuar", "Ok");
                return;
            }
            
            
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

            if (validateEmailRegex.IsMatch(EntryEmail.Text) == false)
            {
                await DisplayAlert("Mensaje", "El correo ingresado no es válido, corríjalo para continuar", "Ok");
                return;
            }
            
            
            
            
            
            
            
            
            //Actualizar usuario
            var editUser = _user.CreateDeepCopy();
            editUser.Email = EntryEmail.Text;
            editUser.Password = "";
            
            var userResponse = await UserService.Instance.PutUserEmail(editUser);
            if (userResponse == null)
            {
                await DisplayAlert("Error", "No se pudo actualizar el usuario", "Ok");
                return;
            }
            
           

            //Actualizar perfil 
            var editProfile = _profile.CreateDeepCopy();
            editProfile.Gender = PickerGenre.SelectedItem.ToString();
            editProfile.BirthDate = DatePickerBirthdate.Date.AddDays(1);
            
            var profileResponse = await ProfileService.Instance.PutProfileByProfileAndId(editProfile, editProfile.Id);
            if (profileResponse == null)
            {
                await DisplayAlert("Error", "No se pudo actualizar el perfil", "Ok");
                return;
            }
            
            //Actualizar especialista
            var editSpecialist = _specialist.CreateDeepCopy();
            editSpecialist.Specialty = EntryEspeciality.Text;
            
            
            var specialistResponse = await SpecialistService.Instance.PutSpecialistBySpecialistAndId(editSpecialist, editSpecialist.Id);
            if (specialistResponse == null)
            {
                await DisplayAlert("Error", "No se pudo actualizar el especialista", "Ok");
                
            }
            
           

            UpdateReferencedObjects();

            GeneralProfilePage.Model.AddCollectionOfSpecialistElements(_profile,_specialist, ConvertToEntity.ConvertToUserEntity(_user));

            
            await Navigation.PopAsync();
        }
        
        private void UpdateReferencedObjects()
        {
            //Actualizamos en la referencia
            _user.Email = EntryEmail.Text;
            
            //Actualizamos en la referencia
            _profile.Gender = PickerGenre.SelectedItem.ToString();
            _profile.BirthDate = DatePickerBirthdate.Date.AddDays(1);
            
            
            //Actualizamos referencia
            _specialist.Specialty = EntryEspeciality.Text;
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}