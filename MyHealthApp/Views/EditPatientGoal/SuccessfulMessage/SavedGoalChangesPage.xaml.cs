using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal.SuccessfulMessage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedGoalChangesPage : Popup
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
            //await Navigation.PopAsync();
        }
    }
}