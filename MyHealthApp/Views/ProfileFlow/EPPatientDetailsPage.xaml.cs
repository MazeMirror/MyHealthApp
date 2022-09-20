﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class EPPatientDetailsPage : ContentPage
    {
        private readonly Profile _profile;
        private readonly Patient _patient;
        private readonly User _user;
        public EPPatientDetailsPage(Profile localProfile,Patient localPatient,User localUser)
        {
            InitializeComponent();
            _profile = localProfile;
            _patient = localPatient;
            _user = localUser;
            CompleteFields();
        }

        private void CompleteFields()
        {
            DatePickerBirthdate.Date = _profile.BirthDate;
            PickerGenre.SelectedItem = _profile.Gender;
            EntryHeight.Text = _patient.Height.ToString(CultureInfo.CurrentCulture);
            EntryWeight.Text = _patient.Weight.ToString(CultureInfo.CurrentCulture);
            EntryPhone.Text = _patient.EmergencyPhone.ToString();
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
            
            //Actualizar paciente
            var editPatient = _patient.CreateDeepCopy();
            
            if (!string.IsNullOrWhiteSpace(EntryHeight.Text))
            {
                try
                {
                    var height = Double.Parse(EntryHeight.Text);
                    editPatient.Height = height;
                }
                catch (FormatException e1)
                {
                    editPatient.Height = 0.0;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(EntryWeight.Text))
            {
                try
                {
                    var weight = Double.Parse(EntryWeight.Text);
                    editPatient.Weight = weight;
                }
                catch (FormatException e2)
                {
                    editPatient.Weight = 0.0;
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
            
            await UpdateSqlLiteRegisters();
          
            ProfilePage.Model.AddCollectionOfPatientElements(_profile,_patient, ConvertToEntity.ConvertToUserEntity(_user));

            
            await Navigation.PopAsync();


        }
        
        private void UpdateReferencedObjects()
        {
            //Actualizamos en la referencia
            _user.Email = EntryEmail.Text;
            
            //Actualizamos en la referencia
            _profile.Gender = PickerGenre.SelectedItem.ToString();
            _profile.BirthDate = DatePickerBirthdate.Date;
            
            
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