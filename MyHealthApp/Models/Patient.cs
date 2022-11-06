using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models
{
    public class Patient :INotifyPropertyChanged, IPrototype<Patient>
    {
        private long _id;
        private long _profileId;
        private double _height;
        private double _weight;
        private long _emergencyPhone;
        
        public long Id { get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        public long ProfileId { get => _profileId;
            set
            {
                _profileId = value;
                OnPropertyChanged();
            }
        }
        
        public double Height { get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        } // Entero ?
        public double Weight { get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        } // Entero ?
        public long EmergencyPhone { get => _emergencyPhone;
            set
            {
                _emergencyPhone = value;
                OnPropertyChanged();
            }
        } //String ?
        //public IList<Specialist> Specialists { get; set; } = new List<Specialist>();
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Patient CreateDeepCopy()
        {
            var patient = (Patient)MemberwiseClone();
            return patient;
        }
    }
}