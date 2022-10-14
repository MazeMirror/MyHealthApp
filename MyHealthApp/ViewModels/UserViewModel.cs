using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealthApp.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _user;
        private int _completedGoals;
        private int _length;

        public ObservableCollection<User> Users
        {
            get => _user;
            set
            {
                if (_user == value) return;
                _user = value;
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

        public UserViewModel(List<User> users)
        {
            _user = new ObservableCollection<User>();

            foreach (var item in users)
            {
                AddUserProfileToList(item);
            }

        }

        public void AddUserProfileToList(User dg)
        {
            
            _user.Add(dg);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
