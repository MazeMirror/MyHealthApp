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
        private String _description;
		public EditMealPlanPage (MealPlan mealPlan)
		{
			InitializeComponent ();

            _mealPlan = mealPlan;
            NameMealPlan.Text=_mealPlan.Name;
            _description = _mealPlan.Description;
            SetDayMPtoBlank();
            DescriptionSplit();

        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            Dismiss(1);
        }

        public void SetDayMPtoBlank()
        {
            LunesMp.Text = "---"; MartesMP.Text = "---"; MiercolesMP.Text = "---";
            JuevesMP.Text = "---"; ViernesMP.Text = "---"; SabadoMP.Text = "---"; DomingoMP.Text = "---";
        }

        public void DescriptionSplit()
        {
            char[] separador = { '-' };

            String[] strList = _description.Split(separador);

            for (int i = 0; i < strList.Length; i++)
            {                
                if (i == 1) LunesMp.Text = "* " + strList[1];
                if (i == 2) MartesMP.Text = "* " + strList[2];
                if (i == 3) MiercolesMP.Text = "* " + strList[3];
                if (i == 4) JuevesMP.Text = "* " + strList[4];
                if (i == 5) ViernesMP.Text = "* " + strList[5];
                if (i == 6) SabadoMP.Text = "* " + strList[6];
                if (i == 7) DomingoMP.Text = "* " + strList[7];
            }
        }

        

        private async void UpdateMealPlan_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new UpdateMealPlanPage(_mealPlan));
        }

        private async void DeleteMealPlan_Clicked(object sender, EventArgs e)
        {
            var result = await Navigation.ShowPopupAsync(new DeleteMealPlanPlage(_mealPlan));
            if (result != null && (int)result == 2)
            {
                await Navigation.PopAsync();
            }

        }
    }
}