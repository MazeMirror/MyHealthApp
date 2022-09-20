using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EPSpecialistDetailsPage : ContentPage
    {
        private readonly Profile _profile;
        private readonly Specialist _specialist;
        private readonly User _user;
        public EPSpecialistDetailsPage(Profile localProfile,Specialist localSpecialist,User localUser)
        {
            InitializeComponent();
            
            _profile = localProfile;
            _specialist = localSpecialist;
            _user = localUser;
            CompleteFields();
        }

        private void CompleteFields()
        {
            DatePickerBirthdate.Date = _profile.BirthDate;
            PickerGenre.SelectedItem = _profile.Gender;
            EntryEspeciality.Text = _specialist.Specialty;
            EntryEmail.Text = _user.Email;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_OnClicked(object sender, EventArgs e)
        {
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
            editProfile.BirthDate = DatePickerBirthdate.Date;
            
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
            await UpdateSqlLiteRegisters();
          
            ProfilePage.Model.AddCollectionOfSpecialistElements(_profile,_specialist, ConvertToEntity.ConvertToUserEntity(_user));

            
            await Navigation.PopAsync();
            
            //Activar función rellenar datos observable
            
        }

        private void UpdateReferencedObjects()
        {
            //Actualizamos en la referencia
            _user.Email = EntryEmail.Text;
            
            //Actualizamos en la referencia
            _profile.Gender = PickerGenre.SelectedItem.ToString();
            _profile.BirthDate = DatePickerBirthdate.Date;
            
            
            //Actualizamos referencia
            _specialist.Specialty = EntryEspeciality.Text;
        }
        
        private async Task  UpdateSqlLiteRegisters()
        {
            //Actualizar SqlLite de user
            await App.SqLiteDb.UpdateProfileAsync(ConvertToEntity.ConvertToProfileEntity(_profile));

            //Actualizar SqlLite de profile
            await App.SqLiteDb.UpdateUserAsync(ConvertToEntity.ConvertToUserEntity(_user));
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}