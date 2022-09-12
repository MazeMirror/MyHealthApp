using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAccountTypePage : ContentPage
    {
        public SelectAccountTypePage()
        {
            InitializeComponent();
        }


        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterDataAccountPage());
        }
    }
}