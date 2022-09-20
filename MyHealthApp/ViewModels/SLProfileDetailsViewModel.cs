using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using Xamarin.Forms;

namespace MyHealthApp.ViewModels
{
    public class SlProfileDetailsViewModel :INotifyPropertyChanged
    {
        private ObservableCollection<ItemProfileModel> _items;
        
        public ObservableCollection<ItemProfileModel> Items
        {
            get { return _items;}
            set
            {
                Items = value;
                OnPropertyChanged();
            } 
        }// this is for the BindableLayout.ItemsSource
        /*IList<ItemProfileModel> source = new List<ItemProfileModel>()
        {
            new ItemProfileModel() { TitleText = "1"},
            new ItemProfileModel() { TitleText = "2"},
            new ItemProfileModel() { TitleText = "3"},
            new ItemProfileModel() { TitleText = "4"},
            new ItemProfileModel() { TitleText = "5"},
        };*/
        
        public ICommand ChangeTextCommand { get; set; }

        public void AddElementToCollection(ItemProfileModel item)
        {
            _items.Add(item);
            OnPropertyChanged();
        }
        
        public void ClearElementsCollection()
        {
            _items.Clear();
            OnPropertyChanged();
        }
        

        public void AddCollectionOfSpecialistElements(Profile profile, Specialist specialist, UserEntity user)
        {
            ClearElementsCollection();
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Edad: {(DateTime.Today.Year - profile.BirthDate.Year).ToString()} Años"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Especialidad: {specialist.Specialty}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Correo: {user.Email}"});
            
            /*IList<ItemProfileModel> source = new List<ItemProfileModel>()
            {
                new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"},
                new ItemProfileModel() { TitleText = $"Edad: {(DateTime.Today.Year - profile.BirthDate.Year).ToString()} Años"},
                new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Especialidad: {specialist.Specialty}"},
                new ItemProfileModel() { TitleText = $"Correo: {user.Email}"},
            };*/
            
            //_items = new ObservableCollection<ItemProfileModel>(source);
            
            OnPropertyChanged();
        }



        public SlProfileDetailsViewModel(Profile profile, Specialist specialist, UserEntity user)
        {
            IList<ItemProfileModel> source = new List<ItemProfileModel>()
            {
                new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"},
                new ItemProfileModel() { TitleText = $"Edad: {(DateTime.Today.Year - profile.BirthDate.Year).ToString()} Años"},
                new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Especialidad: {specialist.Specialty}"},
                new ItemProfileModel() { TitleText = $"Correo: {user.Email}"},
            };
            
            _items = new ObservableCollection<ItemProfileModel>(source);
            
            /*ChangeTextCommand = new Command(() => { 
                foreach (var itemModel in Items)
                {
                    itemModel.TitleText = "New Text";
                }
            });*/
        }
        
        public SlProfileDetailsViewModel(Profile profile, Patient patient , UserEntity user)
        {
            IList<ItemProfileModel> source = new List<ItemProfileModel>()
            {
                new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"},
                new ItemProfileModel() { TitleText = $"Edad: {(DateTime.Today.Year - profile.BirthDate.Year).ToString()} Años"},
                new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"},
                new ItemProfileModel() { TitleText = $"Estatura: {patient.Height.ToString(CultureInfo.CurrentCulture)} cm"},
                new ItemProfileModel() { TitleText = $"Peso: {patient.Weight.ToString(CultureInfo.CurrentCulture)} Kg"},
                new ItemProfileModel() { TitleText = $"Correo: {user.Email}"},
                new ItemProfileModel() { TitleText = $"Contacto de emergencia: +51 {patient.EmergencyPhone.ToString()}"},
            };
            
            _items = new ObservableCollection<ItemProfileModel>(source);
            

            /*ChangeTextCommand = new Command(() => { 
                foreach (var itemModel in Items)
                {
                    itemModel.TitleText = "New Text";
                }
            });*/
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void AddCollectionOfPatientElements(Profile profile, Patient patient, UserEntity user)
        {
            ClearElementsCollection();
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Género: {profile.Gender}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Edad: {(DateTime.Today.Year - profile.BirthDate.Year).ToString()} Años"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Fecha de nacimiento: {profile.BirthDate.ToString(CultureInfo.CurrentCulture)}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Estatura: {patient.Height.ToString(CultureInfo.CurrentCulture)} cm"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Peso: {patient.Weight.ToString(CultureInfo.CurrentCulture)} Kg"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Correo: {user.Email}"});
            AddElementToCollection(new ItemProfileModel() { TitleText = $"Contacto de emergencia: +51 {patient.EmergencyPhone.ToString()}"});
           
            OnPropertyChanged();
        }
    }
}