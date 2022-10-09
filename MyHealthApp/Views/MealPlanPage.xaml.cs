using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.EditPatientPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealPlanPage : ContentPage
    {
        public static PatientMealPlansViewModel MealPlansViewModel;
        private List<MealPlan> _mealPlan;
        private long _patientId;


        public MealPlanPage()
        {
            InitializeComponent();
            getDataMealPlanById();
        }

        private async void getDataMealPlanById()
        {
            _patientId = long.Parse(Application.Current.Properties["PatientId"].ToString());
            _mealPlan = await MealPlanService.Instance.GetMealPlansByPatientId(_patientId);
            MealPlansViewModel = new PatientMealPlansViewModel(_mealPlan);
            StackLayoutMealPlans.BindingContext = MealPlansViewModel;
        }

        private async void SpecificMealPlan_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                var mealPlan = param as MealPlan;

                if (mealPlan != null)
                {
                    await Navigation.ShowPopupAsync(new GetMealPlanPage(mealPlan));
                }
            }
        }


    }
}