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

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDailyGoalPage : ContentPage
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

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_Clicked(object sender, EventArgs e)
        {
            
            double quantityGoal = 0.0;

            _patientId = _dailyGoal.PatientId;

            try
            {
                quantityGoal = Double.Parse(EntryGoalUpdate.Text);
            }
            catch (Exception exception)
            {
                await DisplayAlert("Advertencia", "Ingrese solo números en el campo de meta", "Ok");
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
                Navigation.ShowPopup(new SavedGoalChangesPage());
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Mensaje", "No se pudo actualizar el Daily Goal", "Ok");
            }
            
            
            //await Navigation.PopAsync();
        }
    }
}