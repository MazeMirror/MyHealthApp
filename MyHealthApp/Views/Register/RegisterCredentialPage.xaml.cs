using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterCredentialPage : ContentPage
    {
        public RegisterCredentialPage()
        {
            InitializeComponent();
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SuccessfulRegisterPage());
        }

        private void SecondPasswordButton_OnClicked(object sender, EventArgs e)
        {
            this.SecondPasswordEntry.IsPassword = !this.SecondPasswordEntry.IsPassword;
            this.SecondPasswordButton.Text = this.SecondPasswordEntry.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();
        }

        private void FirstPasswordButton_OnClicked(object sender, EventArgs e)
        {
            this.FirstPasswordEntry.IsPassword = !this.FirstPasswordEntry.IsPassword;
            this.FirstPasswordButton.Text = this.FirstPasswordEntry.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();

        }
    }
}