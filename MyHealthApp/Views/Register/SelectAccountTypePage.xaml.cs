using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit;

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

        private void ButtonPatient_OnClicked(object sender, EventArgs e)
        {
            //Xamarin.CommunityToolkit.Effects.ShadowEffect
            this.LabelPatient.TextColor = Color.FromHex("#FF9467");
            this.LabelDoctor.TextColor = Color.White;
            this.NextPageButton.IsEnabled = true;
        }


        private void ButtonDoctor_OnClicked(object sender, EventArgs e)
        {
            this.LabelDoctor.TextColor = Color.FromHex("#FF9467");
            this.LabelPatient.TextColor = Color.White;
            this.NextPageButton.IsEnabled = true;
        }
    }
}