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

namespace MyHealthApp.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulRegisterPage : ContentPage
    {
        private readonly User _user;
        private readonly Profile _profile;
        public static List<WeeklyGoal> WeeklyGoals;
        public static List<DailyGoal> DailyGoals;
        public SuccessfulRegisterPage(User user, Profile profile)
        {
            InitializeComponent();
            LabelTitle.Text = $"Hola, {profile.Name}\n bienvenido a";
            
            _user = user;
            _profile = profile;
        }

        private async void NextButton_OnClicked(object sender, EventArgs e)
        {
            //Efectuamos el guardado en SQLlite
            //user y profile
            //
            //User user = await UserService.Instance.PostAuthenticateUser(_user.Email, _user.Password);
            //Profile profile = await ProfileService.Instance.GetProfileByUserId(user.Id);

            await App.SqLiteDb.SaveProfileAsync(new ProfileEntity()
            {
                Id = _profile.Id,
                Gender = _profile.Gender,
                Name = _profile.Name,
                LastName = _profile.LastName,
                BirthDate = _profile.BirthDate,
                ImageUrl = _profile.ImageUrl,
                RoleId = _profile.RoleId,
                UserId = _profile.UserId
            });

            await App.SqLiteDb.SaveUserAsync(new UserEntity()
            {
                Id = _user.Id,
                Email = _user.Email
            });

            if (_profile.RoleId == 1)
            {
                var patient = await PatientService.Instance.GetPatientByProfileId(_profile.Id);
                DailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(patient.Id);
                WeeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(patient.Id);
                await Navigation.PushAsync(new TabbedPatient());
            }
            else await Navigation.PushAsync(new TabbedSpecialist());
            
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}