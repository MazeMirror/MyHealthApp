using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models
{
    public interface IPrototype<T>
    {
        T CreateDeepCopy();
    }
    
    public class Profile :INotifyPropertyChanged, IPrototype<Profile>
    {
        private long _id;
        private long _userId;
        private long _roleId;
        private string _name;
        private string _lastName;
        private string _gender;
        private string _imageUrl;
        private DateTime _birthDate;
        //private int _years;
        
        
        public long Id { get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public long UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public long RoleId { get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        public int Years
        {
            get
            {
                var years = DateTime.Now.Year - _birthDate.Year;
                return years;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Profile CreateDeepCopy()
        {
            var profile = (Profile)MemberwiseClone();
            return profile;
        }
    }
}