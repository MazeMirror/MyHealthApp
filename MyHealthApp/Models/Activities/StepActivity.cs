using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models.Activities
{
    public class StepActivity :INotifyPropertyChanged
    {
        private long _id;

        private double _quantity;

        private double _total;

        private long _patientId;

        private double _percentage;

        private DateTime _date;
        
        public long Id { get => _id;
            set
            {
                _id = value;
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
        
        public double Total { get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
            }
        }
        
        public DateTime Date { get => _date;
            set
            {
                _date = value;
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
        
        public void CalculatePercentage()
        {
            double calc = Quantity / Total;
            Percentage = Math.Round(calc, 2);
        }
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}