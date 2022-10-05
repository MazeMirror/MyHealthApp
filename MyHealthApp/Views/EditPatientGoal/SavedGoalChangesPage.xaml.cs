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
    public partial class SavedGoalChangesPage : ContentPage
    {
        public SavedGoalChangesPage()
        {
            InitializeComponent();
            /*texto.Text = _dailyGoal1.Quantity.ToString();
            texto1.Text=_dailyGoal1.Id.ToString();
            texto2.Text=_dailyGoal1.ActivityId.ToString();
            texto.Text=quantityGoal.ToString();*/
        }

        private async void ReturnToDetailsPage(object sender, EventArgs e)
        {
            //await Navigation.PopAsync();
            await Navigation.PopAsync();
            await Navigation.PopAsync();
        }
    }
}