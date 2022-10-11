using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models
{
    public class WeeklyGoal :INotifyPropertyChanged, IPrototype<WeeklyGoal>
    {
        private long _id;

        private double _quantity;

        private double _progress;

        private double _percentage;

        private long _patientId;

        private long _activityId;
        
        private string _descriptionObjective;
        
        private string _descriptionProgress;
        
        private DateTime _startDate;
        private DateTime _endDate;
        
        public long Id { get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }
        
        public string DescriptionObjective { get => _descriptionObjective;
            set
            {
                _descriptionObjective = value;
                OnPropertyChanged();
            }
        }
        
        public string DescriptionProgress { get => _descriptionProgress;
            set
            {
                _descriptionProgress = value;
                OnPropertyChanged();
            }
        }
        
        public double Quantity { get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        
        public double Progress { get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
        
        
        public double Percentage { get => _percentage;
            set
            {
                _percentage = value;
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
        
        public long ActivityId { get => _activityId;
            set
            {
                _activityId = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
        
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public WeeklyGoal CreateDeepCopy()
        {
            var weeklyGoal = (WeeklyGoal)MemberwiseClone();
            return weeklyGoal;
        }

        public void CalculatePercentage()
        {
            double calc = Progress / Quantity;
            Percentage = Math.Round(calc, 2);
        }
    }
}