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
    public partial class TabbedSpecialist : TabbedPage
    {
        public TabbedSpecialist()
        {
            InitializeComponent();
            Application.Current.Properties["RoleLogged"] = 2;
        }

        /*protected override bool OnBackButtonPressed()
        {
            return true;
        }*/
    }
}