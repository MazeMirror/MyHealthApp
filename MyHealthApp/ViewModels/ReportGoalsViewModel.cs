using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class ReportGoalsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ReportGoal> _reportGoals;

        public ReportGoalsViewModel()
        {
            _reportGoals = new ObservableCollection<ReportGoal>();
        }
        

        public ObservableCollection<ReportGoal> ReportGoals
        {
            get => _reportGoals;
            set
            { 
                if(_reportGoals == value) return;
                _reportGoals = value;
                OnPropertyChanged();
            }
        }
        
        public void AddReportGoalToList(ReportGoal rg)
        {
            CompleteDescriptionRg(rg);
            _reportGoals.Add(rg);
            OnPropertyChanged();
        }
        
        public void ClearElementsCollection()
        {
            _reportGoals.Clear();
            OnPropertyChanged();
        }
        
        private void CompleteDescriptionRg(ReportGoal item)
        {
            switch (item.ActivityId)
            {
                case 1:
                {
                    item.Description = "Pasos \nrealizados";
                    item.Unity = "";
                    item.ImageSource = "shoe2.png";
                }; break;
                case 2:
                {
                    item.Description = "Calorías \nquemadas";
                    item.Unity = "kcal";
                    item.ImageSource = "fire.png";
                } ; break;
                case 3:
                {
                    item.Description = "Distancia \nrecorrida";
                    item.Unity = "m";
                    item.ImageSource = "map-maker.png";
                } ; break;
            }
        }
        
        

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}