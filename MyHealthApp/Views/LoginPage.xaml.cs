using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.Register;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void PasswordButton_OnClicked(object sender, EventArgs e)
        {
            this.PasswordEntry.IsPassword = !this.PasswordEntry.IsPassword;
            this.PasswordButton.Text = this.PasswordEntry.IsPassword ? ((char)0xf070).ToString() : ((char)0xf06e).ToString();
        }

        private async void RegisterLabel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectAccountTypePage());
        }
    }
}