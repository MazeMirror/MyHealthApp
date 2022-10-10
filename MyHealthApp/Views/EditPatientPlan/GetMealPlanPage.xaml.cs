using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Extensions;
using MyHealthApp.Models;

namespace MyHealthApp.Views.EditPatientPlan
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GetMealPlanPage : Popup
	{
        private MealPlan _mealPlan;
        public GetMealPlanPage (MealPlan mealPlan)
		{
			InitializeComponent ();

            _mealPlan = mealPlan;
            NameMealPlan.Text = _mealPlan.Name;
            MPDays.Text = _mealPlan.Description;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }
    }
}