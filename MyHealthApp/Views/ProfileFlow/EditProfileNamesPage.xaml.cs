using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfileNamesPage : ContentPage
    {
        public EditProfileNamesPage()
        {
            //Esto sí funcionaaaaaa ProfilePage.Model.AddElementToCollection(new ItemProfileModel(){TitleText = "Holaaa"});
            InitializeComponent();
        }
    }
}