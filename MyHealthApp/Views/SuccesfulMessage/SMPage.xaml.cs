using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.SuccesfulMessage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SMPage : Popup
	{
        private string _message1;
        private string _message2;
        private bool _image1;
        private bool _image2;
        public SMPage (string message1, string message2, bool image1, bool image2)
		{
			InitializeComponent ();
            _message1 = message1;
            _message2 = message2;
            _image1 = image1;
            _image2 = image2;

            PrimaryText.Text = _message1;
            ImageMP.IsVisible = _image1;
            ImageOD_W.IsVisible = _image2;
            SecondText.Text = _message2;          
        }
    }
}