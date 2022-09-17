using System.Collections.Generic;
using System.Drawing;
using MyHealthApp.ViewModels;

namespace MyHealthApp.Helpers
{
    public static class Globals
    {

        public static DevicePageViewModel DevicePageViewModel { get; private set; }


        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static Color PrimaryColor { get; set; } = Xamarin.Forms.Color.LightBlue;
        public static Color SecondaryColor { get; set; } = Xamarin.Forms.Color.CadetBlue;
        public static Color LightTextColor { get; set; } = Xamarin.Forms.Color.Black;
        
        public static Dictionary<string, string> FormatDictionary;
        public static Dictionary<string, string> LanguageDictionary;

        public static void BuildGlobals()
        {
            
            DevicePageViewModel = new DevicePageViewModel();
           

            LanguageDictionary = new Dictionary<string, string>
            {
                {"Nederlands", "nl-NL"},
                {"English", "en-EN"},
                {"Deutsch", "de-DE"}
            };
        }
    }
}