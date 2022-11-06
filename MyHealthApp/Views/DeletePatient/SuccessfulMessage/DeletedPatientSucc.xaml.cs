using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.DeletePatient.SuccessfulMessage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeletedPatientSucc : Popup
    {
        public DeletedPatientSucc()
        {
            InitializeComponent();
        }
    }
}