using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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

        public void ClearProfileList()
        {
            _profiles.Clear();
            OnPropertyChanged();
        }
        
        public void DeletePatientProfileOnList(Profile pro)
        {
            var newList = _profiles.Where(dg => dg.Id != pro.Id).ToList();
            
            ClearProfileList();
            foreach (var dg in newList)
            {
                _profiles.Add(dg);
                OnPropertyChanged();
            }
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