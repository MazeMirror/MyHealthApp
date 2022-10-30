using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangesSavedWeek : ContentPage
    {
        private WeeklyGoal _weeklyGoal;
        public ChangesSavedWeek(double quantityGoal, WeeklyGoal weeklyGoal)
        {
            InitializeComponent();
            _weeklyGoal = weeklyGoal;
            text.Text = "El objetivo semanal se ha actualizado";
            /*texto.Text = _dailyGoal1.Quantity.ToString();*/
            
            //text3.Text = quantityGoal.ToString();
        }

        private async void returnToDetailsPage(object sender, EventArgs e)
        {
            //await Navigation.PopAsync();
            //await Shell.Current.GoToAsync(nameof(PatientDetailsPage));
            await Navigation.PopToRootAsync();
        }
    }
}