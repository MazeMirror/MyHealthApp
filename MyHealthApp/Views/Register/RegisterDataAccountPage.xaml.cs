using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterDataAccountPage : ContentPage
    {
        public RegisterDataAccountPage()
        {
            InitializeComponent();
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterCredentialPage());
        }
    }
}