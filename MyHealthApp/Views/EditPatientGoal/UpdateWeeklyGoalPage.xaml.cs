using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
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
    public partial class UpdateWeeklyGoalPage : ContentPage
    {
        private long _patientId;
        private WeeklyGoal _weeklyGoal;
        private long selecionadoId;
        private String name;
        private String unitDaily;
        public UpdateWeeklyGoalPage(WeeklyGoal weeklyGoal)
        {
            InitializeComponent();
            _weeklyGoal = weeklyGoal;
            selecionadoId = weeklyGoal.ActivityId;


            if (selecionadoId == 1)
            {
                name = "Pasos";
                unitDaily = "pasos";
            }

            if (selecionadoId == 2)
            {
                name = "Caminata";
                unitDaily = "minutos";
            }

            if (selecionadoId == 3)
            {
                name = "Distancia";
                unitDaily = "m";
            }

            SeleccionObjetivoSemanal.Title = name;
            LabelActivitySelectedWeek.Text = unitDaily;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChangesWeekly_Clicked(object sender, EventArgs e)
        {
            double quantityGoal = 0.0;

            _patientId = _weeklyGoal.PatientId;

            try
            {
                quantityGoal = Double.Parse(EntryGoalUpdate.Text);
            }
            catch (Exception exception)
            {
                await DisplayAlert("Advertencia", "Ingrese solo números en el campo de meta", "Ok");
                return;
            }
            _weeklyGoal.Quantity = quantityGoal;
            
            if (_weeklyGoal.Quantity < _weeklyGoal.Progress)
            {
                _weeklyGoal.Progress = _weeklyGoal.Quantity;
            }

            var weeklyGoalResponse = await WeeklyGoalService.Instance.PutWeeklyGoalByPatientId(_patientId, _weeklyGoal);
            if (weeklyGoalResponse != null)
            {
                PatientDetailsPage.WeeklyGoalViewModel.UpdateWeeklyGoalOnList(_weeklyGoal);
                Navigation.ShowPopup(new SavedGoalChangesPage());
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Mensaje", "No se pudo actualizar el Weekly Goal", "Ok");
            }
            
        }
    }
}