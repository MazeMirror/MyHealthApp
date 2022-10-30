using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Views.Register;
using MyHealthApp.Views.SuccesfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.AddPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDailyGoalPage : ContentPage
    {
        private List<Activity> _activities;
        private long _patientId;
        public AddDailyGoalPage(long patientId)
        {
            InitializeComponent();
            _patientId = patientId;
            AssignActivitiesToPickerSource();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void AssignActivitiesToPickerSource()
        {
            _activities = new List<Activity>()
            {
                new Activity() { Id = 1, Name = "Pasos",Unit = "pasos"},
                new Activity() { Id = 2, Name = "Kilocalorias",Unit = "kcal"},
                new Activity() { Id = 3, Name = "Distancia",Unit = "m"}
            };

            PickerActivity.ItemsSource = _activities;
            PickerActivity.SelectedIndex = 0;
        }

        private void PickerActivity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = PickerActivity.SelectedIndex;
            LabelActivitySelected.Text = _activities[selectedIndex].Unit;
        }

        private void PickerActivity_OnTappedChevron(object sender, EventArgs e)
        {
            PickerActivity.Focus();
        }

        private async void AddGoalButton_OnClicked(object sender, EventArgs e)
        {
            var activityId = _activities[PickerActivity.SelectedIndex].Id;
            double quantityGoal = 0.0;
            
            try
            {
                quantityGoal = Double.Parse(EntryGoal.Text);
            }
            catch (Exception exception)
            { 
                await DisplayAlert("Advertencia", "Ingrese solo números en el campo de meta", "Ok"); 
                return;
            }

            if(quantityGoal < 0.0)
            {
                await DisplayAlert("Advertencia", "El numero ingresado debe ser mayor a 0", "Ok");
                return;
            }

            var dailyGoal = new DailyGoal()
            {
                Date = DateTime.Today,
                Quantity = quantityGoal,
                Progress = 0.0,
                ActivityId = activityId,
            };

            var dailyGoalResponse = await DailyGoalService.Instance.PostDailyGoalByPatientId(_patientId, dailyGoal);

            if (dailyGoalResponse == null)
            {
                await DisplayAlert("Error", "No se pudo crear el Daily Goal para este paciente", "Ok");
                return;
            }
            
            PatientDetailsPage.DailyGoalsViewModel.AddDailyGoalToList(dailyGoalResponse);
            await Navigation.PopAsync();
            Navigation.ShowPopup(new SMPage(4));
        }
    }
}