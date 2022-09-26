using System;
using MyHealthApp.Views.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            FallbackIfWelcomeIsStartPageByInterruptError();
        }

        private async void StartButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void FallbackIfWelcomeIsStartPageByInterruptError()
        {
            await App.SqLiteDb.DeleteAllProfileAsync();
            await App.SqLiteDb.DeleteAllUsersAsync();
        }
    }
}