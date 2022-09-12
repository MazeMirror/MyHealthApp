using System;
using MyHealthApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Archivo-Bold.ttf", Alias = "ArchivoBold")]
[assembly: ExportFont("Archivo-SemiBold.ttf", Alias = "ArchivoSemiBold")]
[assembly: ExportFont("FjallaOne-Regular.ttf", Alias = "FjallaOne")]
[assembly: ExportFont("Archivo-Regular.ttf", Alias = "ArchivoRegular")]
namespace MyHealthApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new WelcomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
