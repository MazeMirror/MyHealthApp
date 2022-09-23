using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models
{
    public class DailyGoal :INotifyPropertyChanged, IPrototype<DailyGoal>
    {
        private long _id;

        private double _quantity;

        private double _progress;
        
        private double _percentage;

        private long _patientId;

        private long _activityId;

        private string _descriptionObjective;
        private string _descriptionProgress;
        
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
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public DailyGoal CreateDeepCopy()
        {
            var dailyGoal = (DailyGoal)MemberwiseClone();
            return dailyGoal;
        }
    }
}