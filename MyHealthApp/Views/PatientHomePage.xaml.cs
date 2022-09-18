using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Services.MiBand;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientHomePage : ContentPage
    {
        private readonly string _propertyKey = "LastConnectedDevice";
        public PatientHomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        { 
            //App.RequestLocationPermission();
            if (Windesheart.PairedDevice == null)
                return;
           
        }
        
        private void SetApplicationProperties()
        {
            if (Windesheart.PairedDevice != null)
            {
                App.Current.Properties[_propertyKey] = Windesheart.PairedDevice.Uuid;
            }
        }

        //Handle Auto-connect to the last connected device with App-properties
        private async Task HandleAutoConnect()
        {
            var knownGuid = App.Current.Properties[_propertyKey].ToString();
            if (!string.IsNullOrEmpty(knownGuid))
            {
                var knownDevice = await Windesheart.GetKnownDevice(Guid.Parse(knownGuid));
                knownDevice.Connect(CallbackHandler.OnConnect);
            }
        }

        private async void RegisterSmartwatch_OnClicked(object sender, EventArgs e)
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new DevicePage());
            await Navigation.PushAsync(new DevicePage());
        }
    }
}