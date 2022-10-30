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
        private int _messageType;
        public SMPage (int messageType)
		{
			InitializeComponent ();
            _messageType = messageType;
            SetMessageText();
        }

        private void SetMessageText()
        {
            switch (_messageType)
            {
                // Plan alimenticio
                case 1:
                    {
                        PrimaryText.Text = "Añadido exitosamente";
                        ImageMP.IsVisible = true;
                        SecondText.Text = "El plan alimenticio se ha agregado";
                    }; break;
                case 2:
                    {
                        PrimaryText.Text = "Cambios Guardados";
                        ImageMP.IsVisible = true;
                        SecondText.Text = "Plan alimenticio modificado correctamente";
                    }; break;
                case 3:
                    {
                        PrimaryText.Text = "El Plan se ha eliminado";
                        ImageMP.IsVisible = true;
                        SecondText.Text = "";
                    }; break;

                // Objetivo diario
                case 4:
                    {
                        PrimaryText.Text = "Añadido exitosamente";
                        ImageOD_W.IsVisible = true;
                        SecondText.Text = "El plan objetivo diario se ha establecido";
                    }; break;
                case 5:
                    {
                        PrimaryText.Text = "Cambios Guardados";
                        ImageOD_W.IsVisible = true;
                        SecondText.Text = "Objetivo modificado correctamente";
                    }; break;
                case 6:
                    {
                        PrimaryText.Text = "El objetivo se ha eliminado";
                        ImageOD_W.IsVisible = true;
                        SecondText.Text = "";
                    }; break;

                // Objetivo semanal, se usa el 5 y 6 para mostrar el mismo mensaje en obj. semanal
                case 7:
                    {
                        PrimaryText.Text = "Añadido exitosamente";
                        ImageOD_W.IsVisible = true;
                        SecondText.Text = "El plan objetivo semanal se ha establecido";
                    }; break;
            }
        }
    }
}