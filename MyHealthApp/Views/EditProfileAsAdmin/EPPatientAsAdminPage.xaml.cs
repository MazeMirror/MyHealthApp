using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class EPPatientAsAdminPage : ContentPage
    {
        private Profile _profile;
        private Patient _patient;
        private User _user;
        public EPPatientAsAdminPage(Profile profile)
        {
            InitializeComponent();
            _profile = profile;
            CompleteFields();
        }
        
        private async void CompleteFields()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                _patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
                _user = await UserService.Instance.GetUserById(_profile.UserId);
                
                
                DatePickerBirthdate.Date = _profile.BirthDate;
                PickerGenre.SelectedItem = _profile.Gender;
                EntryHeight.Text = _patient.Height.ToString(CultureInfo.CurrentCulture);
                EntryWeight.Text = _patient.Weight.ToString(CultureInfo.CurrentCulture);
                EntryPhone.Text = _patient.EmergencyPhone.ToString();
                EntryEmail.Text = _user.Email;
            });
           
            
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Advertencia",
                    "El campo de email no puede estar vacío", "Ok");
                return;
            }
            
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");

            if (validateEmailRegex.IsMatch(EntryEmail.Text) == false)
            {
                await DisplayAlert("Mensaje", "El correo ingresado no es válido, corríjalo para continuar", "Ok");
                return;
            }

            if (EntryPhone.Text.Length < 9)
            {
                await DisplayAlert("Mensaje", "El número de celular no debe ser inferior a 9 digitos, corríjalo para continuar", "Ok");
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
            
            //Actualizar paciente
            var editPatient = _patient.CreateDeepCopy();
            
            if (!string.IsNullOrWhiteSpace(EntryHeight.Text))
            {
                try
                {
                    var height = Double.Parse(EntryHeight.Text);
                    if (height == 0.0)
                    {
                        await DisplayAlert("Mensaje", "La altura no puede ser 0 cm", "Ok");
                        return;
                    }
                    
                    editPatient.Height = height;
                }
                catch (FormatException e1)
                {
                    await DisplayAlert("Mensaje", "La altura debe ser una unidad válida, corríjalo para continuar", "Ok");
                    return;
                }
            }
           
            
            if (!string.IsNullOrWhiteSpace(EntryWeight.Text))
            {
                try
                {
                    var weight = Double.Parse(EntryWeight.Text);
                    if (weight == 0.0)
                    {
                        await DisplayAlert("Mensaje", "El peso no puede ser 0 kg", "Ok");
                        return;
                    }
                    editPatient.Weight = weight;
                }
                catch (FormatException e2)
                {
                    await DisplayAlert("Mensaje", "El peso debe ser una unidad válida, corríjalo para continuar", "Ok");
                    return;
                }
                
            }
            
            if (!string.IsNullOrWhiteSpace(EntryPhone.Text))
            {
                try
                {
                    var phone = long.Parse(EntryPhone.Text);
                    editPatient.EmergencyPhone = phone;
                }
                catch (FormatException e3)
                {
                    editPatient.EmergencyPhone = 0;
                }
            }
            else
            {
                editPatient.EmergencyPhone = 0;
            }
            
            
            var patientResponse = await PatientService.Instance.PutPatientByPatientAndId(editPatient, editPatient.Id);
            if (patientResponse == null)
            {
                await DisplayAlert("Error", "No se pudo actualizar el paciente", "Ok");
                
            }
            
            //Actualizamos en la referencia
            _patient.Height = editPatient.Height;
            _patient.Weight = editPatient.Weight;
            _patient.EmergencyPhone = editPatient.EmergencyPhone;


            UpdateReferencedObjects();
            
            GeneralProfilePage.Model.AddCollectionOfPatientElements(_profile,_patient, ConvertToEntity.ConvertToUserEntity(_user));

            
            await Navigation.PopAsync();
        }
        
        private void UpdateReferencedObjects()
        {
            //Actualizamos en la referencia
            _user.Email = EntryEmail.Text;
            
            //Actualizamos en la referencia
            _profile.Gender = PickerGenre.SelectedItem.ToString();
            _profile.BirthDate = DatePickerBirthdate.Date.AddDays(1);
        }
        
    }
}