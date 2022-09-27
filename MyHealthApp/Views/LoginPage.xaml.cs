﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using MyHealthApp.Views.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static List<WeeklyGoal> WeeklyGoals;
        public static List<DailyGoal> DailyGoals;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void PasswordButton_OnClicked(object sender, EventArgs e)
        {
            this.EntryPassword.IsPassword = !this.EntryPassword.IsPassword;
            this.PasswordButton.Text = this.EntryPassword.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();
        }

        private async void RegisterLabel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectAccountTypePage());
        }

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text) ||
                string.IsNullOrWhiteSpace(EntryPassword.Text))
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

            if (EntryPassword.Text.Length < 8)
            {
                await DisplayAlert("Mensaje", "La contraseña ingresada no debe ser inferior a 8 caracteres", "Ok");
                return;
            }
            
            
            

            //Hacemos la peticion al backend para autenticar
            //Y recibimos el perfil asociado al user
            User user = await UserService.Instance.PostAuthenticateUser(EntryEmail.Text, EntryPassword.Text);
            if (user == null)
            {
                await DisplayAlert("Credenciales inválidos", "El correo o la contraseña ingresada es incorrecta", "Ok");
                return;
            }

            Profile profile = await ProfileService.Instance.GetProfileByUserId(user.Id);
            if (profile == null) return;
            
            //Si tenemos exito en ello, guardamos en SQLlite los datos de perfil (PerfilId, UserCorreo y datos de perfil) 
            //2 entidades: Profile y User (sin contraseña obviamente)
            await App.SqLiteDb.SaveProfileAsync(new ProfileEntity()
            {
                Id = profile.Id,
                Gender = profile.Gender,
                Name = profile.Name,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate,
                ImageUrl = profile.ImageUrl,
                RoleId = profile.RoleId,
                UserId = profile.UserId
            });

            await App.SqLiteDb.SaveUserAsync(new UserEntity()
            {
                Id = user.Id,
                Email = user.Email
            });
            
            
            //Dependiendo del rolID de perfil mandamos el Tabbed de patient O Specialist
            if (profile.RoleId == 1)
            {
                var patient = await PatientService.Instance.GetPatientByProfileId(profile.Id);
                DailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(patient.Id);
                WeeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(patient.Id);
                await Navigation.PushAsync(new TabbedPatient());
            }
            else await Navigation.PushAsync(new TabbedSpecialist());
        }
    }
}