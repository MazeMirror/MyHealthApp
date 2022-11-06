using System;
using MyHealthApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAccountTypePage : ContentPage
    {
        private Role _role;
        public SelectAccountTypePage()
        {
            InitializeComponent();
            _role = new Role();
        }


        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterDataAccountPage(_role));
        }

        private void ButtonPatient_OnClicked(object sender, EventArgs e)
        {
            //Xamarin.CommunityToolkit.Effects.ShadowEffect
            this.LabelPatient.TextColor = Color.FromHex("#FF9467");
            this.LabelDoctor.TextColor = Color.White;
            this.NextPageButton.IsEnabled = true;
            _role.Id = 1;
        }


        private void ButtonDoctor_OnClicked(object sender, EventArgs e)
        {
            this.LabelDoctor.TextColor = Color.FromHex("#FF9467");
            this.LabelPatient.TextColor = Color.White;
            this.NextPageButton.IsEnabled = true;
            _role.Id = 2;
        }
    }
}