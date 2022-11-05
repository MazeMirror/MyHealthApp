﻿using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Views.EditPatientPlan.SuccessfullMessage;
using MyHealthApp.Views.SuccesfulMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientPlan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateMealPlanPage : Popup
    {
        private long _patientId;
        private MealPlan _mealPlan;
        private String countDescription;

        public UpdateMealPlanPage(MealPlan mealPlan)
        {
            InitializeComponent();
            _mealPlan = mealPlan;
            CounterTextUpdate.Text = 0.ToString();
            getDataMealPlan();
        }

        private void getDataMealPlan()
        {
            
            PlanNameUpdate.Text = _mealPlan.Name;
            PlanDescriptionUpdate.Text = _mealPlan.Description;
        }
        private void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void SaveChanges_Clicked(object sender, EventArgs e)
        {
            if (PlanDescriptionUpdate.Text == null || PlanDescriptionUpdate.Text == "")
            {
                Navigation.ShowPopup(new SMPage("Advertencia", "El campo esta vacio", false, false));
                //PlanDescriptionUpdate.Text = "El campo esta vacio";
                return;
            }
            else
            {
                _patientId = _mealPlan.PatientId;
                _mealPlan.Description = PlanDescriptionUpdate.Text;

                var mealPlanResponse = await MealPlanService.Instance.PutMealPlanByPatientId(_patientId, _mealPlan);

                if (mealPlanResponse != null)
                {
                    //ACTUALIZAR LISTA.....de mealPlans
                    //PatientDetailsPage.MealPlansViewModel.Update(_dailyGoal);
                    //await Navigation.PopAsync();
                    Dismiss(2);
                    Navigation.ShowPopup(new SMPage("Cambios Guardados", "Plan alimenticio modificado correctamente", true, false));

                }
            }
        }

        private void CountTextUpdate(object sender, TextChangedEventArgs e)
        {
            countDescription = PlanDescriptionUpdate.Text.ToString();
            CounterTextUpdate.Text = countDescription.Length.ToString();
        }
    }
}