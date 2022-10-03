using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Helpers;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [System.Runtime.InteropServices.Guid("A63B9823-FB43-4942-BAAA-5F02EAF86AC8")]
    public partial class DevicePage : ContentPage
    {
        public static ListView DeviceList;
        public static Button ScanButton;
        //private readonly string _propertyKey = "LastConnectedDevice";
        

        public DevicePage()
        {
            InitializeComponent();
            BindingContext = Globals.DevicePageViewModel;

            BuildPage();
        }

        protected override void OnAppearing()
        {
            App.RequestLocationPermission();
            if (Windesheart.PairedDevice == null) return;
        }
        
        private async void ReturnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            //PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Dispositivos", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton2(absoluteLayout, this,ReturnButton_Clicked);

            ScanButton = PageBuilder.AddButton(absoluteLayout, "", Globals.DevicePageViewModel.ScanButtonClicked, 0.05, 0.25, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Color.FromHex("#FF7E10"));
            ScanButton.SetBinding(Button.TextProperty, "ScanButtonText");
            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.25, 50, 50, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);
            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.25, 50, 50, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);
            PageBuilder.AddLabel(absoluteLayout, "", 0.80, 0.25, Globals.LightTextColor, "StatusText", 14);
            
            #region device ListView
            var deviceTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},

                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},
                    }
                };

                Label label = new Label { TextColor = Color.Black, FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center,FontSize = 12,};
                label.SetBinding(Label.TextProperty, "Device.Name");
                grid.Children.Add(label, 0, 0);


                Label label2 = new Label { TextColor = Color.Black, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontAttributes = FontAttributes.Italic, FontSize = 12 };
                label2.SetBinding(Label.TextProperty, "Rssi");

                grid.Children.Add(label2, 2, 0);

                Label label3 = new Label { TextColor = Color.Black, Text = "Signal strength:", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 };
                grid.Children.Add(label3, 1, 0);

                return new ViewCell { View = grid };
            });
            DeviceList = new ListView { BackgroundColor = Color.White, ItemTemplate = deviceTemplate };
            DeviceList.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedDevice", BindingMode.TwoWay));
            DeviceList.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(DeviceList, new Rectangle(0.5, 0.55, 0.90, 0.4));
            AbsoluteLayout.SetLayoutFlags(DeviceList, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(DeviceList);
            #endregion

            #region disconnectButton
            Button disconnectButton = PageBuilder.AddButton(absoluteLayout, "Desconectar", Globals.DevicePageViewModel.DisconnectButtonClicked, 0.05, 0.82, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Color.Red);
            #endregion
        }
        
        protected override void OnDisappearing()
        {
            SetApplicationProperties();
            Globals.DevicePageViewModel.OnDisappearing();
        }
        
        private void SetApplicationProperties()
        {
            if (Windesheart.PairedDevice != null)
            {
                Application.Current.Properties["LastConnectedDevice"] = Windesheart.PairedDevice.Uuid;
            }
        }
    }
}