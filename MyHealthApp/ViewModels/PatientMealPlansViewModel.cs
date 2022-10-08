using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyHealthApp.ViewModels
{
    public class PatientMealPlansViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MealPlan> _mealPlans;
        private int _completedPlans;
        private int _length;

        public ObservableCollection<MealPlan> MealPlans
        {
            get => _mealPlans;
            set
            {
                if (_mealPlans == value) return;
                _mealPlans = value;
                OnPropertyChanged();
            }
        }

        public int CompletedPlans
        {
            get => _completedPlans;
            set
            {
                _completedPlans = value;
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

        // Verificar si exsite item
        public bool ItemAlreadyExist(MealPlan pro)
        {
            bool exist = false;

            foreach (var item in MealPlans)
            {
                if (item.Id == pro.Id) exist = true;
            }

            return exist;
        }

        // Agregar MealPlan a lista
        public void AddDailyGoalToList(MealPlan mp)
        {
            _mealPlans.Add(mp);
            OrderObservableList();
            //CalculateCompletedGoals();
            Lenght = _mealPlans.Count;
            //OnPropertyChanged();
        }

        private void OrderObservableList()
        {
            var orderedList = _mealPlans.OrderBy(mp => mp.Name).ToList();
            ClearMealPlanList();

            foreach (var item in orderedList)
            {
                _mealPlans.Add(item);
                OnPropertyChanged();
            }

        }

        public void ClearMealPlanList()
        {
            _mealPlans.Clear();
            OnPropertyChanged();
        }

        public PatientMealPlansViewModel(List<MealPlan> mealPlans)
        {
            _mealPlans = new ObservableCollection<MealPlan>();

            foreach (var item in mealPlans)
            {
                //CompleteDescriptionDg(item);
                AddDailyGoalToList(item);
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    } 
}
