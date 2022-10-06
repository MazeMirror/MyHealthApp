using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal.SuccessfulMessage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletedGoalPage : ContentPage
    {
        public DeletedGoalPage()
        {
            InitializeComponent();
        }

        private async void ReturnToDetailsPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}