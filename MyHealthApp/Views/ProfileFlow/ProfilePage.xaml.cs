using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            
            var stack = new StackLayout
            {
                Padding = new Thickness(20, 17, 20, 17),
                BackgroundColor = Color.Aqua,
                Children =
                {
                    new Label
                    {
                        Text = "Hola"
                    }
                }
            };

            this.StackLayoutProfileDetails = stack;
           
        }

        private void LabelEditName_OnTapped(object sender, EventArgs e)
        {
            
        }

        private void LabelEditDetails_OnTapped(object sender, EventArgs e)
        {
            
        }

        private void LabelDeleteAccount_OnTapped(object sender, EventArgs e)
        {
            
        }
    }
}