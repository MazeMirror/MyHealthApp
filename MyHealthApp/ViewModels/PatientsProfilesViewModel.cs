using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class PatientsProfilesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Profile> _profiles;

        public ObservableCollection<Profile> Profiles
        {
            get => _profiles;
            set
            { 
                if(_profiles == value) return;
                _profiles = value;
                OnPropertyChanged();
            }
        }

        public bool ItemAlreadyExist(Profile pro)
        {
            bool exist = false;

            foreach (var item in Profiles)
            {
                if (item.Id == pro.Id) exist = true;
            }

            return exist;
        }

        public void AddProfileToList(Profile pro)
        {
            _profiles.Add(pro);
            OnPropertyChanged();
        }

        public PatientsProfilesViewModel()
        {
            _profiles = new ObservableCollection<Profile>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       
    }
}