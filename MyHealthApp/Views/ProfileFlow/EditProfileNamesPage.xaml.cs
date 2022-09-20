using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Models;
using MyHealthApp.Models.SqlLite;
using MyHealthApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.ProfileFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfileNamesPage : ContentPage
    {
        public EditProfileNamesPage()
        {
            //Esto sí funcionaaaaaa ProfilePage.Model.AddElementToCollection(new ItemProfileModel(){TitleText = "Holaaa"});
            InitializeComponent();
            
            EntryName.Text = ProfilePage.LocalProfile.Name;
            EntryLastName.Text = ProfilePage.LocalProfile.LastName;
            
            //Los mismo que arriba pero como Xamarin Senior :V
            //EntryName.BindingContext = ProfilePage.LocalProfile;
            //EntryLastName.BindingContext = ProfilePage.LocalProfile;
            //EntryName.SetBinding(Entry.TextProperty,new Binding(){Path = "Name" ,Mode = BindingMode.OneWay});
            //EntryLastName.SetBinding(Entry.TextProperty,new Binding(){Path = "LastName",Mode = BindingMode.OneWay });
        }

        private async void LabelCancel_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveChanges_OnClicked(object sender, EventArgs e)
        {
            var profile = ProfilePage.LocalProfile.CreateDeepCopy();
            
            profile.Name =  EntryName.Text;
            profile.LastName = EntryLastName.Text;

            var updatedProfile = await ProfileService.Instance.PutProfileByProfileAndId(profile,profile.Id);
            if (updatedProfile == null)
            {
                await DisplayAlert("Error", "No se pudo actualizar el perfil, vuelva a intentarlo", "Ok");
            }
            else
            {
                ProfilePage.LocalProfile.Name = EntryName.Text;
                ProfilePage.LocalProfile.LastName = EntryLastName.Text;
                
                //await App.SqLiteDb.DeleteAllProfileAsync();
                //await App.SqLiteDb.SaveProfileAsync(ConvertToEntity.ConvertToProfileEntity(updatedProfile));
                await App.SqLiteDb.UpdateProfileAsync(ConvertToEntity.ConvertToProfileEntity(ProfilePage.LocalProfile));
                await Navigation.PopAsync();
            }
           
        }
    }
}