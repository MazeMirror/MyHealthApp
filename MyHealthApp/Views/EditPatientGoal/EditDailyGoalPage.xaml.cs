using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditPatientGoal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDailyGoalPage : ContentPage
    {
        private DailyGoal _dailyGoal;
        public EditDailyGoalPage(DailyGoal dailyGoal)
        {
            InitializeComponent();
            //Para trabajar con la referencia
            _dailyGoal = dailyGoal;
            LabelGoal.BindingContext = _dailyGoal;
            LabelProgress.BindingContext = _dailyGoal;
            
            ProgressRing.BindingContext = _dailyGoal;

            //NOTA: Para trabajar sin referencia, a tu criterio si lo amerita 
            //_dailyGoal = dailyGoal.CreateDeepCopy()
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private async void UpdateDailyGoal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateDailyGoalPage(_dailyGoal));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await Navigation.ShowPopupAsync(new DeleteDailyGoalPage(_dailyGoal));
            if (result != null && (int)result == 2)
            {
                await Navigation.PopAsync();
            }

        }
    }
}