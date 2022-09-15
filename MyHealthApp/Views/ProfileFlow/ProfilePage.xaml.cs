﻿using System;
using System.Collections.Generic;
using MyHealthApp.Models;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private SlProfileDetailsViewModel _model;
        public ProfilePage()
        {
            InitializeComponent();
            
            GetDataInformation();

        }

        private void GetDataInformation()
        {
            //Hacemos peticion a SQLlite para obtener el perfil y el user
            
            //ilustrativo
            Random rnd = new Random();
            int num = rnd.Next(2);
            var profile = new Profile()
            {
                Gender = "Masculino",
                Name = "Josias",
                LastName = "Olaya",
                ImageUrl = "https://....",
                BirthDate = DateTime.Now,
                RoleId = 1, 
            };

            var user = new User()
            {
                Email = "JosiasOlaya@hotmail.com"
            };
            ////////////////////////////////////////////////////////

            if (profile.RoleId == 0)
            {
                //Hacemos peticion al backend para obtener el paciente por RoleId
                var patient = new Patient()
                {
                    Height = 35.2,
                    Weight = 45.5,
                    EmergencyPhone = 912457857,
                };
                
                BindingContext = new SlProfileDetailsViewModel(profile, patient, user);
            }
            else
            {
                //Hacemos peticion al backend para obtener el especialista por RoleId
                
                var specialist = new Specialist()
                {
                    Specialty = "Pediatria",
                };
                
                BindingContext = new SlProfileDetailsViewModel(profile, specialist, user);
            }
        }
        

        private void LabelEditName_OnTapped(object sender, EventArgs e)
        {
            
        }

        private void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {
            
        }

        private void LabelDeleteAccount_OnTapped(object sender, EventArgs e)
        {
            
        }
    }
}