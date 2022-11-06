using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.Collections;
using Microcharts;
using MyHealthApp.Models;
using MyHealthApp.Models.Activities;
using MyHealthApp.Services.Activities;
using MyHealthApp.ViewModels;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeekReportPage : ContentPage
    {
        private readonly ObservableCollection<WeeklyGoal> _weeklyGoalsObservable;
        private readonly ReportGoalsViewModel _reportGoalsViewModel;
        private long _patientId;
        public CurrentWeekReportPage(ObservableCollection<WeeklyGoal> weeklyGoals, long patientId)
        {
            InitializeComponent();
            _weeklyGoalsObservable = weeklyGoals;
            _reportGoalsViewModel = new ReportGoalsViewModel();
            FlexLayoutWeeklyInform.BindingContext = _reportGoalsViewModel;
            FrameWeeklyGoals.BindingContext = _weeklyGoalsObservable;
            _patientId = patientId;
            
            DateTime date = DateTime.Today;
            int day = (int)date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day == 0 ? day : 7 - day));

            LabelCurrentWeekStart.Text = monday.ToString("d MMM", new CultureInfo("es-ES"));
            LabelCurrentWeekEnd.Text = sunday.ToString("d MMM", new CultureInfo("es-ES"));

            //WeeklyStepBar.IsVisible = true;
            ChangeColorButtonsOnClick(1);
            ChangeFiltersByButtonClicked(1);

            CalculateInformForCurrentWeek();
            GetDataOfWeek();
        }

        private void ButtonStep_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(1);
            ChangeFiltersByButtonClicked(1);

        }

        private void ButtonDistance_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(2);
            ChangeFiltersByButtonClicked(2);

        }

        private void ButtonWalks_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(3);
            ChangeFiltersByButtonClicked(3);
        }

        private void ChangeColorButtonsOnClick(int id)
        {
            switch (id)
            {
                case 1:
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ButtonStep.BackgroundColor = Color.FromHex("#FF9162");
                            ButtonStep.TextColor = Color.White;
                            ButtonDistance.BackgroundColor = Color.White;
                            ButtonDistance.TextColor = Color.Black;
                            ButtonWalks.BackgroundColor = Color.White;
                            ButtonWalks.TextColor = Color.Black;
                        });
                    }; break;
                case 2:
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ButtonDistance.BackgroundColor = Color.FromHex("#FF9162");
                            ButtonDistance.TextColor = Color.White;
                            ButtonStep.BackgroundColor = Color.White;
                            ButtonStep.TextColor = Color.Black;
                            ButtonWalks.BackgroundColor = Color.White;
                            ButtonWalks.TextColor = Color.Black;
                        });
                    }; break;
                case 3:
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ButtonWalks.BackgroundColor = Color.FromHex("#FF9162");
                            ButtonWalks.TextColor = Color.White;
                            ButtonDistance.BackgroundColor = Color.White;
                            ButtonDistance.TextColor = Color.Black;
                            ButtonStep.BackgroundColor = Color.White;
                            ButtonStep.TextColor = Color.Black;
                        });
                    }; break;
            }
        }

        private void ChangeFiltersByButtonClicked(int id)
        {
            switch (id)
            {
                case 1:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        WeeklyStepBar.IsVisible = true;
                        WeeklyDistanceBar.IsVisible = false;
                        WeeklyKilocalorieBar.IsVisible = false;
                    });

                }; break;
                case 2:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        WeeklyStepBar.IsVisible = false;
                        WeeklyDistanceBar.IsVisible = true;
                        WeeklyKilocalorieBar.IsVisible = false;
                    });

                }; break;
                case 3:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        WeeklyStepBar.IsVisible = false;
                        WeeklyDistanceBar.IsVisible = false;
                        WeeklyKilocalorieBar.IsVisible = true;
                    });
                }; break;
            }
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void CalculateInformForCurrentWeek()
        {
            var weeklyGoals = new List<WeeklyGoal>();

            foreach (var item in _weeklyGoalsObservable)
            {
                weeklyGoals.Add(item);
                var patientID = weeklyGoals.First().PatientId;
            }
            
            var listSteps = weeklyGoals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = weeklyGoals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = weeklyGoals.Where(dg => dg.ActivityId == 3).ToList();

            double maxQuantity = 0;
            double maxProgress = 0;
            
            if (!listSteps.IsEmpty())
            {
                foreach (var item in listSteps)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 1
                });
            }
            
            //reiniciamos maximos
            maxQuantity = 0;
            maxProgress = 0;

            if (!listCalories.IsEmpty())
            {
                foreach (var item in listCalories)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 2
                });
            }
            
            //reiniciamos maximos
            maxQuantity = 0;
            maxProgress = 0;

            if (!listDistances.IsEmpty())
            {
                foreach (var item in listDistances)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }
                
                _reportGoalsViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            
            
        }

        private async void GetDataOfWeek()
        {
            string ActivityDay = null;
            var _chartEntriesStepActivity = new List<ChartEntry>();
            var _chartEntriesDistanceActivity = new List<ChartEntry>();
            var _chartEntriesKilocalorieActivity = new List<ChartEntry>();

            List<StepActivity> listStepRecordPerWeek = new List<StepActivity>();
            List<DistanceActivity> listDistancesPerWeek = new List<DistanceActivity>();
            List<KilocalorieActivity> listKilocaloriesPerWeek = new List<KilocalorieActivity>();

            StepActivity StepActivityDay = new StepActivity();
            DistanceActivity DistanceActivityDay = new DistanceActivity();
            KilocalorieActivity KilocalorieActivityDay = new KilocalorieActivity();

            DateTime datePrueba = DateTime.Now;
            var firstDayOfWeek = new DateTime(datePrueba.Year, datePrueba.Month, datePrueba.Day);

            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Monday) firstDayOfWeek = firstDayOfWeek.AddDays(0);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Tuesday) firstDayOfWeek = firstDayOfWeek.AddDays(-1);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Wednesday) firstDayOfWeek = firstDayOfWeek.AddDays(-2);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Thursday) firstDayOfWeek = firstDayOfWeek.AddDays(-3);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Friday) firstDayOfWeek = firstDayOfWeek.AddDays(-4);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Saturday) firstDayOfWeek = firstDayOfWeek.AddDays(-5);
            if (firstDayOfWeek.DayOfWeek == DayOfWeek.Sunday) firstDayOfWeek = firstDayOfWeek.AddDays(-6);

            for (int i = 0; i < 7; i++)
            {
                if (i == 0) ActivityDay = "L";
                if (i == 1) ActivityDay = "M";
                if (i == 2) ActivityDay = "X";
                if (i == 3) ActivityDay = "J";
                if (i == 4) ActivityDay = "V";
                if (i == 5) ActivityDay = "S";
                if (i == 6) ActivityDay = "D";

                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    listStepRecordPerWeek = await
                        StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, firstDayOfWeek.AddDays(i), firstDayOfWeek.AddDays(i));

                    listDistancesPerWeek = await
                        DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, firstDayOfWeek.AddDays(i), firstDayOfWeek.AddDays(i));

                    listKilocaloriesPerWeek = await
                        KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, firstDayOfWeek.AddDays(i), firstDayOfWeek.AddDays(i));
                });
                // StepsActivity
                if (listStepRecordPerWeek.Count > 0)
                {
                    StepActivityDay = listStepRecordPerWeek.First();
                    var entriesStep = new ChartEntry(Convert.ToSingle(StepActivityDay.Quantity))
                    {
                        ValueLabel = StepActivityDay.Quantity.ToString(),
                        Label = ActivityDay,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesStepActivity.Add(entriesStep);
                }
                else
                {
                    var entriesStep = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityDay,
                        Color = SKColors.Black,
                    };
                    _chartEntriesStepActivity.Add(entriesStep);
                }

                // DistanceActivity
                if (listDistancesPerWeek.Count > 0)
                {
                    DistanceActivityDay = listDistancesPerWeek.First();
                    var entriesDistance = new ChartEntry(Convert.ToSingle(DistanceActivityDay.Quantity))
                    {
                        ValueLabel = DistanceActivityDay.Quantity.ToString(),
                        Label = ActivityDay,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesDistanceActivity.Add(entriesDistance);
                }
                else
                {
                    var entriesDistance = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityDay,
                        Color = SKColors.Black,
                    };
                    _chartEntriesDistanceActivity.Add(entriesDistance);
                }

                // DistanceActivity
                if (listKilocaloriesPerWeek.Count > 0)
                {
                    KilocalorieActivityDay = listKilocaloriesPerWeek.First();
                    var entriesKilocalorie = new ChartEntry(Convert.ToSingle(KilocalorieActivityDay.Quantity))
                    {
                        ValueLabel = KilocalorieActivityDay.Quantity.ToString(),
                        Label = ActivityDay,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesKilocalorieActivity.Add(entriesKilocalorie);
                }
                else
                {
                    var entriesKilocalorie = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityDay,
                        Color = SKColors.Black,
                    };
                    _chartEntriesKilocalorieActivity.Add(entriesKilocalorie);
                }


            }

            WeeklyStepBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesStepActivity,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            WeeklyDistanceBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesDistanceActivity,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            WeeklyKilocalorieBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesKilocalorieActivity,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

        }
    }
}