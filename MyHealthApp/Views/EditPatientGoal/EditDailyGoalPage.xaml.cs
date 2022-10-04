using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
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
            ProgressRing.BindingContext = _dailyGoal;

            //NOTA: Para trabajar sin referencia, a tu criterio si lo amerita 
            //_dailyGoal = dailyGoal.CreateDeepCopy()
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        //Se usa cuando actualizas exitosamente
        private void UpdateDescriptionDailyGoal(DailyGoal item)
        {
            switch (item.ActivityId)
            {
                case 1:
                {
                    item.DescriptionObjective = String.Format("Realizar {0} pasos en el día",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} pasos",item.Progress);
                }; break;
                case 2:
                {
                    item.DescriptionObjective = String.Format("Realizar {0} minutos de caminata",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} minutos",item.Progress);
                } ; break;
                case 3:
                {
                    item.DescriptionObjective = String.Format("Recorrer una distancia de {0} m",item.Quantity);
                    item.DescriptionProgress = String.Format("Progreso: {0} m",item.Progress);
                } ; break;
            }
        }
    }
}