using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class PatientsProfilesViewModel 
    {
        private ObservableCollection<Profile> _profiles;

        public ObservableCollection<Profile> Profiles
        {
            get => _profiles;
            set
            { 
                if(_profiles == value) return;
                _profiles = value;
                
            }
        }
        
        public PatientsProfilesViewModel()
        {
            _profiles = new ObservableCollection<Profile>(){new Profile(){Name = "", LastName = ""}};
        }
    }
}