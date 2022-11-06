using MyHealthApp.Models;
using MyHealthApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.EditProfileAsAdmin.DeleteProfileMD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteProfileSuccess : Popup
    {
        public DeleteProfileSuccess()
        {
            InitializeComponent();
        }
    }
}