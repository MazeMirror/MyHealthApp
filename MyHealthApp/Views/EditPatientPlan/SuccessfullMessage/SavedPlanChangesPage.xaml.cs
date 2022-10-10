using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientPlan.SuccessfullMessage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedPlanChangesPage : Popup
    {
        public SavedPlanChangesPage()
        {
            InitializeComponent();
        }

        private async void ReturnToDetailsPage(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}