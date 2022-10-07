using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyHealthApp.Models
{
    public class ReportGoal :INotifyPropertyChanged, IPrototype<ReportGoal>
    {
        private double _quantity;

        private double _progress;
        private long _activityId;
        private string _description;
        private string _unity;
        private string _imageSource;

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
        
        public long ActivityId
        {
            get => _activityId;
            set
            {
                _activityId = value;
                OnPropertyChanged();
            }
        }
        
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        
        public string Unity
        {
            get => _unity;
            set
            {
                _unity = value;
                OnPropertyChanged();
            }
        }
        


        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ReportGoal CreateDeepCopy()
        {
            var reportGoal = (ReportGoal)MemberwiseClone();
            return reportGoal;
        }
    }
}