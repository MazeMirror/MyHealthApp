using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPatient : TabbedPage
    {
        public TabbedPatient()
        {
            InitializeComponent();
            Application.Current.Properties["RoleLogged"] = 1;
        }

        /*protected override bool OnBackButtonPressed()
        {
            return true;
        }*/
    }
}