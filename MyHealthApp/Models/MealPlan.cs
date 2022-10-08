using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealthApp.Models
{
    public class MealPlan : INotifyPropertyChanged, IPrototype<MealPlan>
    {
        private long _id;
        private string _name;
        private string _description;
        private long _patientId;

        public long Id { get => _id; 
            set 
            { 
                _id = value;
                OnPropertyChanged();
            } 
        }

        public string Name { get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description { get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public long PatientId { get => _patientId;
            set
            {
                _patientId = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MealPlan CreateDeepCopy()
        {
            var mealPlan = (MealPlan)MemberwiseClone();
            return mealPlan;
        }

    }
}
