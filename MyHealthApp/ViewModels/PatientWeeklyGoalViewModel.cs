using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class PatientWeeklyGoalViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<WeeklyGoal> _weeklyGoals;

        public PatientWeeklyGoalViewModel()
        {
            _weeklyGoals = new ObservableCollection<WeeklyGoal>();
        }
        
        public PatientWeeklyGoalViewModel(List<WeeklyGoal> weeklyGoals)
        {
            _weeklyGoals = new ObservableCollection<WeeklyGoal>();
            
            foreach (var item in weeklyGoals)
            {
                CompleteDescriptionWg(item);
                AddWeeklyToList(item);
            }
        }
        
        public ObservableCollection<WeeklyGoal> WeeklyGoals
        {
            get => _weeklyGoals;
            set
            { 
                if(_weeklyGoals == value) return;
                _weeklyGoals = value;
                OnPropertyChanged();
            }
        }
        
        public void AddWeeklyToList(WeeklyGoal wg)
        {
            double calc = wg.Progress / wg.Quantity;
            wg.Percentage = Math.Round(calc, 2);
            CompleteDescriptionWg(wg);
            _weeklyGoals.Add(wg);
            OnPropertyChanged();
        }

        public void ClearWeeklyGoalList()
        {
            _weeklyGoals.Clear();
            OnPropertyChanged();
        }

        private void CompleteDescriptionWg(WeeklyGoal item)
        {
            switch (item.ActivityId)
            {
                case 1:
                {
                    item.DescriptionObjective = String.Format("Realizar {0} pasos en la semana",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} pasos",item.Progress);
                }; break;
                case 2:
                {
                    item.DescriptionObjective = String.Format("Quemar {0} kilocalorías en la semana",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} kilocalorías",item.Progress);
                } ; break;
                case 3:
                {
                    item.DescriptionObjective = String.Format("Recorrer una distancia de {0} m",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} m",item.Progress);
                } ; break;
            }
        }

        
        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //ACTULIZAR DESPUES DE CAMBIO EN ELEMENTO
        public void UpdateWeeklyGoalOnList(WeeklyGoal weeklyGoal)
        {
            foreach (var wg in _weeklyGoals)
            {
                if (wg.Id == weeklyGoal.Id)
                {
                    wg.Quantity = weeklyGoal.Quantity;
                    double calc = wg.Progress / wg.Quantity;
                    wg.Percentage = Math.Round(calc, 2);
                    CompleteDescriptionWg(wg);
                    
                }
            }
            //Despues de actualizar ordenamos la lista
            //Hacer lo mismo para ELIMINAR (solo aplica a dailyGoal)
            
        }

        public void DeleteWeeklyGoalOnList(WeeklyGoal weeklyGoal)
        {
            var newList = _weeklyGoals.Where(wg => wg.Id != weeklyGoal.Id).ToList();
            
            ClearWeeklyGoalList();
            foreach (var wg in newList)
            {
                _weeklyGoals.Add(wg);
                OnPropertyChanged();
            }
            
        }

        public void UpdateDescriptionWg()
        {
            foreach (var item in WeeklyGoals)
            {
                CompleteDescriptionWg(item);
            }
            OnPropertyChanged();
        }
    }
}