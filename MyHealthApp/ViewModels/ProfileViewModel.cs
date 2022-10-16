using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealthApp.ViewModels
{
    public class ProfileViewModel
    {
        private ObservableCollection<Profile> _profile;
        private int _completedGoals;
        private int _length;

        public ObservableCollection<Profile> Profiles
        {
            get => _profile;
            set
            {
                if (_profile == value) return;
                _profile = value;
                OnPropertyChanged();
            }


        }

        public int Lenght
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        public ProfileViewModel(List<Profile> profiles)
        {
            _profile = new ObservableCollection<Profile>();

            foreach (var item in profiles)
            {
                AddUserProfileToList(item);
            }

        }

        public void AddUserProfileToList(Profile dg)
        {
            _profile.Add(dg);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
