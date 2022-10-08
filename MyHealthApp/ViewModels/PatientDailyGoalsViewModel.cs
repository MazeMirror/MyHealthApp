using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MyHealthApp.Models;

namespace MyHealthApp.ViewModels
{
    public class PatientDailyGoalsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<DailyGoal> _dailyGoals;
        private int _completedGoals;
        private int _length;

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

        public int CompletedGoals
        {
            get => _completedGoals;
            set
            {
                _completedGoals = value;
                OnPropertyChanged();
            }
        }

        public int Lenght
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }

        private void CalculateCompletedGoals()
        {
            CompletedGoals = _dailyGoals.Count(goal => goal.Progress == goal.Quantity);
            OnPropertyChanged();
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

        public void AddDailyGoalToList(DailyGoal dg)
        {
            double calc = dg.Progress / dg.Quantity;
            dg.Percentage = Math.Round(calc, 2);
            CompleteDescriptionDg(dg);
            _dailyGoals.Add(dg);
            OrderObservableList();
            CalculateCompletedGoals();
            Lenght = _dailyGoals.Count;
            //OnPropertyChanged();
        }

        private void OrderObservableList()
        {
           var orderedList = _dailyGoals.OrderBy(dg => dg.Quantity).ToList();
           ClearDailyGoalList();
           
           foreach (var item in orderedList)
           {
               _dailyGoals.Add(item);
               OnPropertyChanged();
           }
           
        }
        
        

        public void ClearDailyGoalList()
        {
            _dailyGoals.Clear();
            OnPropertyChanged();
        }

        public PatientDailyGoalsViewModel()
        {
            _dailyGoals = new ObservableCollection<DailyGoal>();
        }

        public PatientDailyGoalsViewModel(List<DailyGoal> dailyGoals)
        {
            _dailyGoals = new ObservableCollection<DailyGoal>();
            
            foreach (var item in dailyGoals)
            {
                CompleteDescriptionDg(item);
                AddDailyGoalToList(item);
            }
            
        }
        
        private void CompleteDescriptionDg(DailyGoal item)
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

        //ACTULIZAR DESPUES DE CAMBIO EN ELEMENTO
        public void UpdateDailyGoalOnList(DailyGoal dailyGoal)
        {
            foreach (var dg in _dailyGoals)
            {
                if (dg.Id == dailyGoal.Id)
                {
                    dg.Quantity = dailyGoal.Quantity;
                    double calc = dg.Progress / dg.Quantity;
                    dg.Percentage = Math.Round(calc, 2);
                    CompleteDescriptionDg(dg);
                    //Si tu metodo no actualiza poner OnPropertyChanged();
                    //Si igual no lo hace 🙏😔
                }
            }
            //Despues de actualizar ordenamos la lista
            //Hacer lo mismo para ELIMINAR (solo aplica a dailyGoal)
            OrderObservableList();
            CalculateCompletedGoals();
        }

        public void DeleteDailyGoalOnList(DailyGoal dailyGoal)
        {
            var newList = _dailyGoals.Where(dg => dg.Id != dailyGoal.Id).ToList();
            
            ClearDailyGoalList();
            foreach (var dg in newList)
            {
                _dailyGoals.Add(dg);
                OnPropertyChanged();
            }
            
            Lenght = _dailyGoals.Count;
            CalculateCompletedGoals();
        }
    }
}