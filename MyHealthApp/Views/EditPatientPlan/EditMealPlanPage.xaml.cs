using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Views.EditPatientGoal.SuccessfulMessage;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyHealthApp.Views.EditPatientGoal;

namespace MyHealthApp.Views.EditPatientPlan
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditMealPlanPage : Popup
    {
		private MealPlan _mealPlan;
		public EditMealPlanPage (MealPlan mealPlan)
		{
			InitializeComponent ();

            _mealPlan = mealPlan;
            NameMealPlan.Text=_mealPlan.Name;
            MPDays.Text = _mealPlan.Description;

        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }
        private async void UpdateMealPlan_Clicked(object sender, EventArgs e)
        {
            Dismiss(1);
            var result = await Navigation.ShowPopupAsync(new UpdateMealPlanPage(_mealPlan));
            if (result != null && (int)result == 2)
            {
                await Navigation.PopAsync();
            }

        }

        private async void DeleteMealPlan_Clicked(object sender, EventArgs e)
        {
            Dismiss(1);
            var result = await Navigation.ShowPopupAsync(new DeleteMealPlanPlage(_mealPlan));
            if (result != null && (int)result == 2)
            {
                await Navigation.PopAsync();
            }

        }
    }
}