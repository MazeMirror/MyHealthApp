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
                    item.Description = "Pasos realizados";
                    item.Unity = "";
                }; break;
                case 2:
                {
                    item.Description = "Calorías quemadas";
                    item.Unity = "kcal";
                } ; break;
                case 3:
                {
                    item.Description = "Distancia recorrida";
                    item.Unity = "m";
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