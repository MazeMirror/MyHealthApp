using System;
using System.IO;
using MyHealthApp.Helpers;
using MyHealthApp.Views;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("Archivo-Bold.ttf", Alias = "ArchivoBold")]
[assembly: ExportFont("Archivo-SemiBold.ttf", Alias = "ArchivoSemiBold")]
[assembly: ExportFont("FjallaOne-Regular.ttf", Alias = "FjallaOne")]
[assembly: ExportFont("Archivo-Regular.ttf", Alias = "ArchivoRegular")]
[assembly: ExportFont("Fcraft-B.ttf", Alias = "FCraftBorgoBold")]
namespace MyHealthApp
{
    public partial class App : Application
    {
        private static SqLiteHelper _db;
        public App()
        {
            InitializeComponent();
            Globals.BuildGlobals();

            //MainPage = new NavigationPage( new WelcomePage());
        }

        public static SqLiteHelper SqLiteDb =>
            _db ??= new SqLiteHelper(
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "Myhealth.db3"));

        protected override void OnStart()
        {
            CheckLogin();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            //CheckLogin();
        }

        private void CheckLogin()
        {
            if (Current.Properties.ContainsKey("RoleLogged"))
            {
                var roleLogged = (int)Current.Properties["RoleLogged"];

                switch (roleLogged)
                {
                    case 1: MainPage = new NavigationPage(new TabbedPatient());
                        break;
                    case 2: MainPage = new NavigationPage(new TabbedSpecialist());
                        break;
                    case 3: MainPage = new NavigationPage(new WelcomePage());
                        break;
                }
                
            }
            else
            {
                MainPage = new NavigationPage(new WelcomePage());
            }
        }
        
        public static async void RequestLocationPermission()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }
        }
        
    }
    
    
}


