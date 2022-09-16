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
        

        public DevicePage()
        {
            InitializeComponent();
           //BindingContext = devicePageViewModel;

            BuildPage();
        }

        protected override void OnAppearing()
        {
            App.RequestLocationPermission();
            if (Windesheart.PairedDevice == null) return;
        }
        
        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            //PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Device", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            ScanButton = PageBuilder.AddButton(absoluteLayout, "", Globals.DevicePageViewModel.ScanButtonClicked, 0.15, 0.25, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
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

                Label label = new Label
                {
                    FontAttributes = FontAttributes.Bold, 
                    VerticalTextAlignment = TextAlignment.Center, 
                    HorizontalTextAlignment = TextAlignment.Center, 
                    FontSize = 12,
                    BackgroundColor = Color.Red,
                    TextColor = Color.Black
                };
                label.SetBinding(Label.TextProperty, "Device.Name");
                grid.Children.Add(label, 0, 0);


                Label label2 = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontAttributes = FontAttributes.Italic, FontSize = 12 };
                label2.SetBinding(Label.TextProperty, "Rssi");

                grid.Children.Add(label2, 2, 0);

                Label label3 = new Label { Text = "Signal strength:", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 };
                grid.Children.Add(label3, 1, 0);

                return new ViewCell { View = grid };
            });
            DeviceList = new ListView { BackgroundColor = Globals.SecondaryColor, ItemTemplate = deviceTemplate };
            DeviceList.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedDevice", BindingMode.TwoWay));
            DeviceList.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(DeviceList, new Rectangle(0.5, 0.55, 0.90, 0.4));
            AbsoluteLayout.SetLayoutFlags(DeviceList, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(DeviceList);
            #endregion

            #region disconnectButton
            Button disconnectButton = PageBuilder.AddButton(absoluteLayout, "Disconnect", Globals.DevicePageViewModel.DisconnectButtonClicked, 0.15, 0.85, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            #endregion
        }
        
        protected override void OnDisappearing()
        {
            Globals.DevicePageViewModel.OnDisappearing();
        }
    }
}