using MyHealthApp.Models;
using MyHealthApp.Services;
using ProgressRingControl.Forms.Plugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyHealthApp.Views.SuccesfulMessage;
using Xamarin.CommunityToolkit.UI.Views;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDailyGoalPage : Popup
    {
        private long _patientId;
        private DailyGoal _dailyGoal;
        private long selecionadoId;
        private String name;
        private String unitDaily;
        public UpdateDailyGoalPage(DailyGoal dailyGoal)
        {
            InitializeComponent();

            _dailyGoal = dailyGoal;
            selecionadoId = _dailyGoal.ActivityId;
            

            if (selecionadoId == 1)
            {
                name = "Pasos";
                unitDaily = "pasos";
            }

            if (selecionadoId == 2)
            {
                name = "Kilocalorias";
                unitDaily = "kcal";
            }

            if (selecionadoId == 3)
            {
                name = "Distancia";
                unitDaily = "m";
            }

            SeleccionObjetivo.Title = name;
            LabelActivitySelected.Text = unitDaily;
            EntryGoalUpdate.Text = _dailyGoal.Quantity.ToString(CultureInfo.InvariantCulture);
        }

        private void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        private async void SaveChanges_Clicked(object sender, EventArgs e)
        {
            if (EntryGoalUpdate.Text == null || EntryGoalUpdate.Text == "")
            {
                Navigation.ShowPopup(new SMPage("Advertencia", "El campo esta vacio", false, false));
                //await DisplayAlert("Advertencia", "El campo esta vacio", "Ok");
                return;
            }

            double quantityGoal = 0.0;

            _patientId = _dailyGoal.PatientId;

            try
            {
                quantityGoal = Double.Parse(EntryGoalUpdate.Text);
            }
            catch (Exception exception)
            {
                Navigation.ShowPopup(new SMPage("Advertencia", "Ingrese solo números en el campo de meta", false, false));
                //await DisplayAlert("Advertencia", "Ingrese solo números en el campo de meta", "Ok");
                return;
            }
            _dailyGoal.Quantity = quantityGoal;

            if (_dailyGoal.Quantity < _dailyGoal.Progress)
            {
                _dailyGoal.Progress = _dailyGoal.Quantity;
            }
           
            
            var dailyGoalResponse = await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _dailyGoal);
            if (dailyGoalResponse != null)
            {
                //ACTUALIZAR LISTA.....de dailyGoals
                PatientDetailsPage.DailyGoalsViewModel.UpdateDailyGoalOnList(_dailyGoal);
                //await Navigation.PopAsync();
                Dismiss(2);
                Navigation.ShowPopup(new SMPage("Cambios Guardados", "Objetivo modificado correctamente", false, true));
            }
            else
            {
                Navigation.ShowPopup(new SMPage("Mensaje", "No se pudo actualizar el Daily Goal", false, false));
                //await DisplayAlert("Mensaje", "No se pudo actualizar el Daily Goal", "Ok");
            }
            
            
            //await Navigation.PopAsync();
        }
    }
}