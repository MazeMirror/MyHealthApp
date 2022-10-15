using MyHealthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EPPatientAsAdminPage : ContentPage
    {
        public EPPatientAsAdminPage(Profile profile)
        {
            InitializeComponent();
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}