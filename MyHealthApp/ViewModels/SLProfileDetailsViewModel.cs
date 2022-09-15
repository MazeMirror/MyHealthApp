using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using MyHealthApp.Models;
using Xamarin.Forms;

namespace MyHealthApp.ViewModels
{
    public class SlProfileDetailsViewModel
    {
        public ObservableCollection<ItemProfileModel> Items { get; set; }// this is for the BindableLayout.ItemsSource
        /*IList<ItemProfileModel> source = new List<ItemProfileModel>()
        {
            new ItemProfileModel() { TitleText = "1"},
            new ItemProfileModel() { TitleText = "2"},
            new ItemProfileModel() { TitleText = "3"},
            new ItemProfileModel() { TitleText = "4"},
            new ItemProfileModel() { TitleText = "5"},
        };*/
        
        public ICommand ChangeTextCommand { get; set; }

        public SlProfileDetailsViewModel(Profile profile, Specialist specialist, User user)
        {
            IList<ItemProfileModel> source = new List<ItemProfileModel>()
            {
                new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"},
                new ItemProfileModel() { TitleText = $"Edad: 50 años"},
                new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Especialidad: {specialist.Specialty}"},
                new ItemProfileModel() { TitleText = $"Correo: {user.Email}"},
            };
            
            Items = new ObservableCollection<ItemProfileModel>(source);
            
            /*ChangeTextCommand = new Command(() => { 
                foreach (var itemModel in Items)
                {
                    itemModel.TitleText = "New Text";
                }
            });*/
        }
        
        public SlProfileDetailsViewModel(Profile profile, Patient patient , User user)
        {
            IList<ItemProfileModel> source = new List<ItemProfileModel>()
            {
                new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"},
                new ItemProfileModel() { TitleText = $"Edad: 8 años"},
                new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel(){TitleText = $"Estatura: {patient.Height.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Peso: {patient.Weight.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Correo: {user.Email}"},
                new ItemProfileModel() { TitleText = $"Contacto de emergencias: {patient.EmergencyPhone.ToString()}"},
            };
            
            Items = new ObservableCollection<ItemProfileModel>(source);
            
            /*ChangeTextCommand = new Command(() => { 
                foreach (var itemModel in Items)
                {
                    itemModel.TitleText = "New Text";
                }
            });*/
        }
        
        
    }
}