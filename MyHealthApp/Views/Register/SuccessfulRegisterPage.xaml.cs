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
    public partial class SuccessfulRegisterPage : ContentPage
    {
        public SuccessfulRegisterPage()
        {
            InitializeComponent();
        }

        private void NextButton_OnClicked(object sender, EventArgs e)
        {
            
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}