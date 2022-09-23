using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class PatientDailyGoalsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DailyGoal> _dailyGoals;

        public ObservableCollection<DailyGoal> DailyGoals
        {
            get => _dailyGoals;
            set
            { 
                if(_dailyGoals == value) return;
                _dailyGoals = value;
                OnPropertyChanged();
            }
        }

        public bool ItemAlreadyExist(DailyGoal pro)
        {
            bool exist = false;

            foreach (var item in DailyGoals)
            {
                if (item.Id == pro.Id) exist = true;
            }

            return exist;
        }

        public void AddDailyGoalToList(DailyGoal pro)
        {
            _dailyGoals.Add(pro);
            OnPropertyChanged();
        }

        public void ClearDailyGoalList()
        {
            _dailyGoals.Clear();
            OnPropertyChanged();
        }

        public PatientDailyGoalsViewModel(List<DailyGoal> dailyGoals)
        {
            _dailyGoals = new ObservableCollection<DailyGoal>();
            
            foreach (var item in dailyGoals)
            {
                switch (item.ActivityId)
                {
                    case 1:
                    {
                        item.DescriptionObjective = String.Format("Realizar {0} pasos en el día",item.Quantity);
                        item.DescriptionProgress = String.Format("Progreso: {0} pasos",item.Progress);
                    }; break;
                    case 2:
                    {
                        item.DescriptionObjective = String.Format("Realizar {0} minutos de caminata",item.Quantity);
                        item.DescriptionProgress = String.Format("Progreso: {0} minutos",item.Progress);
                    } ; break;
                    case 3:
                    {
                        item.DescriptionObjective = String.Format("Recorrer una distancia de {0} m",item.Quantity);
                        item.DescriptionProgress = String.Format("Progreso: {0} m",item.Progress);
                    } ; break;
                }
                _dailyGoals.Add(item);
                //AddDailyGoalToList(item.CreateDeepCopy());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void AdjustProgressPercentages()
        {
            foreach (var dg in _dailyGoals)
            {
                double calc = dg.Progress / dg.Quantity;
                dg.Percentage = Math.Round(calc, 2);
                OnPropertyChanged();
            }
            
        }

        
    }
}