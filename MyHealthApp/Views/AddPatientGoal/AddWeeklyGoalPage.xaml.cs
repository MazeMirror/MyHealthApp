using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.AddPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWeeklyGoalPage : ContentPage
    {
        private List<Activity> _activities;
        private long _patientId;
        
        public AddWeeklyGoalPage(long patientId)
        {
            InitializeComponent();
            _patientId = patientId;
            AssignActivitiesToPickerSource();
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

            var weeklyGoal = new WeeklyGoal()
            {
                Quantity = quantityGoal,
                Progress = 0.0,
                ActivityId = activityId
            };

            var weeklyGoalResponse = await WeeklyGoalService.Instance.PostWeeklyGoalByPatientId(_patientId, weeklyGoal);

            if (weeklyGoalResponse == null)
            {
                await DisplayAlert("Error", "No se pudo crear el Weekly Goal para este paciente", "Ok");
                return;
            }
            
            PatientDetailsPage.WeeklyGoalViewModel.AddWeeklyToList(weeklyGoalResponse);
            await Navigation.PopAsync();
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

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}