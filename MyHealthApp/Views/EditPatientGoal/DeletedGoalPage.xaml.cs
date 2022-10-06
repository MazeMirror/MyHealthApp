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