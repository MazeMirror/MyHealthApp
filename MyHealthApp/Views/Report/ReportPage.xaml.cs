using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.Collections;
using MyHealthApp.Models;
using MyHealthApp.Models.Activities;
using MyHealthApp.Services;
using MyHealthApp.Services.Activities;
using MyHealthApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Entry = Microcharts.ChartEntry;
//using Microcharts;
using SkiaSharp;
using Microcharts;

namespace MyHealthApp.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        private long _patientId;
        private readonly ReportGoalsViewModel _reportGoalsDailyViewModel;

        private readonly ReportGoalsViewModel _reportGoalsWeeklyViewModel;

        private readonly PatientDailyGoalsViewModel _dailyGoalsViewModel;
        
        private readonly ReportGoalsViewModel _reportGoalsMonthlyViewModel;

        private readonly PatientWeeklyGoalViewModel _weeklyGoalViewModel;

        //private readonly PlotModelViewModel _plotModelViewModel;

        //public List<ChartEntry> _chartEntries;

        private List<DailyGoal> dailyGoalsSteps;
        private List<DailyGoal> dailyGoalsDistance;
        private List<DailyGoal> dailyGoalsWalks;
        private List<WeeklyGoal> weeklyGoalsSteps;

        private List<StepActivity> _listStepActivities;


        public ReportPage()
        {
            InitializeComponent();
            
            
            _reportGoalsDailyViewModel = new ReportGoalsViewModel();
            _reportGoalsWeeklyViewModel = new ReportGoalsViewModel();
            _reportGoalsMonthlyViewModel = new ReportGoalsViewModel();
            _dailyGoalsViewModel = new PatientDailyGoalsViewModel();
            _weeklyGoalViewModel = new PatientWeeklyGoalViewModel();
            //_plotModelViewModel = new PlotModelViewModel();
            dailyGoalsDistance = new List<DailyGoal>();
            dailyGoalsSteps = new List<DailyGoal>();
            dailyGoalsWalks = new List<DailyGoal>();

            _listStepActivities = new List<StepActivity>();

            InitConfigurations();

            //LoadDataStepDistanceWalks();
        }

        protected override void OnAppearing()
        {
            _reportGoalsDailyViewModel.ClearElementsCollection();
            CalculateInformForDay(DatePickerDailyInform.Date);
            _reportGoalsWeeklyViewModel.ClearElementsCollection();
            CalculateInformForMonth(DatePickerMonthInform.Date);
            _reportGoalsMonthlyViewModel.ClearElementsCollection();
            CalculateInformForWeek(DatePickerDateWeekStartInform.Date,DatePickerDateWeekFinishInform.Date);
        }


        private async void InitConfigurations()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                var profile = await App.SqLiteDb.GetProfileAsync();
                var patient = await PatientService.Instance.GetPatientByProfileId(profile.Id);
                if (patient != null)
                {
                    _patientId = patient.Id;
                } 
            });
            
            
            //Layouts de informe
            FlexLayoutDailyInform.BindingContext = _reportGoalsDailyViewModel;
            FlexLayoutWeeklyInform.BindingContext = _reportGoalsWeeklyViewModel;
            FlexLayoutMonthlyInform.BindingContext = _reportGoalsMonthlyViewModel;
            
            //Daily Informe
            //CalculateInformForDay(DatePickerDailyInform.Date);
            DatePickerDailyInform.MaximumDate = DateTime.Today;

            //Weekly informe
            DateTime date = DateTime.Today;
            int day = (int)date.DayOfWeek;
            DateTime monday = date.AddDays((-1) * (day == 0 ? 6 : day - 1));
            DateTime sunday = date.AddDays((1) * (day == 0 ? day : 7 - day));
            
            DatePickerDateWeekStartInform.Date = monday;
            DatePickerDateWeekFinishInform.Date = sunday;
            
            DatePickerDateWeekStartInform.MaximumDate = DatePickerDateWeekFinishInform.Date.AddDays(-1);
            DatePickerDateWeekFinishInform.MinimumDate = DatePickerDateWeekStartInform.Date.AddDays(1);
            
            //Monthly inform
            //CalculateInformForMonth(DatePickerMonthInform.Date);
            
            //Layout daily
            StackLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            //Layout weekly
            StackLayoutWeeklyGoals.BindingContext = _weeklyGoalViewModel;

            //DailyStepBarData.BindingContext = _plotModelViewModel;
        }

        private async void LabelBack_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private void ButtonDay_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(1);
            ChangeFiltersByButtonClicked(1);
            buttonsDashBoard.IsVisible = false;
            dashBoardData.IsVisible = false;
        }

        private void ButtonWeek_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(2);
            ChangeFiltersByButtonClicked(2);
            buttonsDashBoard.IsVisible = true;
            dashBoardData.IsVisible = true;

            ChangeColorButtonsOnClick(4);
            ChangeFiltersByButtonClicked(4);
        }

        private void ButtonMonth_OnClicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(3);
            ChangeFiltersByButtonClicked(3);
            buttonsDashBoard.IsVisible = true;
            dashBoardData.IsVisible = true;

            ChangeColorButtonsOnClick(4);
            ChangeFiltersByButtonClicked(4);
        }

        private void ButtonStep_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(4);
            ChangeFiltersByButtonClicked(4);
        }

        private void ButtonDistance_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(5);
            ChangeFiltersByButtonClicked(5);

        }

        private void ButtonWalks_Clicked(object sender, EventArgs e)
        {
            ChangeColorButtonsOnClick(6);
            ChangeFiltersByButtonClicked(6);
        }

        private void ChangeColorButtonsOnClick(int id)
        {
            switch (id)
            {
                case 1:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonDay.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonDay.TextColor = Color.White;
                        ButtonWeek.BackgroundColor = Color.White;
                        ButtonWeek.TextColor = Color.Black;
                        ButtonMonth.BackgroundColor = Color.White;
                        ButtonMonth.TextColor = Color.Black;
                    });
                }; break;
                case 2:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonWeek.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonWeek.TextColor = Color.White;
                        ButtonDay.BackgroundColor = Color.White;
                        ButtonDay.TextColor = Color.Black;
                        ButtonMonth.BackgroundColor = Color.White;
                        ButtonMonth.TextColor = Color.Black;
                    });   
                }; break;
                case 3:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ButtonMonth.BackgroundColor = Color.FromHex("#FF9162");
                        ButtonMonth.TextColor = Color.White;
                        ButtonWeek.BackgroundColor = Color.White;
                        ButtonWeek.TextColor = Color.Black;
                        ButtonDay.BackgroundColor = Color.White;
                        ButtonDay.TextColor = Color.Black;
                    });
                }; break;

                case 4:
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
                case 5:
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
                case 6:
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
                        StackLayoutDailyInform.IsVisible = true;
                        FrameDailyGoals.IsVisible = true;
                        
                        StackLayoutWeeklyInform.IsVisible = false;
                        FrameWeeklyGoals.IsVisible = false;

                        WeeklyStepBar.IsVisible = false;
                        WeeklyDistanceBar.IsVisible = false;
                        WeeklyKilocalorieBar.IsVisible = false;

                        StackLayoutMonthlyInform.IsVisible = false;

                        MonthlyDistanceBar.IsVisible = false;
                        MonthlyKilocalorieBar.IsVisible = false;
                        MonthlyStepBar.IsVisible = false;
                    });
                    
                } ; break;
                case 2:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StackLayoutDailyInform.IsVisible = false;
                        FrameDailyGoals.IsVisible = false;
                        
                        StackLayoutWeeklyInform.IsVisible = true;
                        FrameWeeklyGoals.IsVisible = true;

                        WeeklyStepBar.IsVisible = true;

                        StackLayoutMonthlyInform.IsVisible = false;

                        MonthlyDistanceBar.IsVisible = false;
                        MonthlyKilocalorieBar.IsVisible = false;
                        MonthlyStepBar.IsVisible = false;
                    });
                    
                } ; break;
                case 3:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StackLayoutDailyInform.IsVisible = false;
                        FrameDailyGoals.IsVisible = false;

                        StackLayoutWeeklyInform.IsVisible = false;
                        FrameWeeklyGoals.IsVisible = false;

                        WeeklyStepBar.IsVisible = false;
                        WeeklyDistanceBar.IsVisible = false;
                        WeeklyKilocalorieBar.IsVisible = false;

                        StackLayoutMonthlyInform.IsVisible = true;

                        MonthlyStepBar.IsVisible = true;
                    });
                    
                } ; break;
                case 4:
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        if (StackLayoutWeeklyInform.IsVisible == true)
                        {
                            WeeklyStepBar.IsVisible = true;
                            WeeklyDistanceBar.IsVisible = false;
                            WeeklyKilocalorieBar.IsVisible = false;
                        }

                        if (StackLayoutMonthlyInform.IsVisible == true)
                        {
                            MonthlyStepBar.IsVisible = true;
                            MonthlyDistanceBar.IsVisible = false;
                            MonthlyKilocalorieBar.IsVisible = false;
                        }
                    });

                    }; break;
                case 5:
                {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (StackLayoutWeeklyInform.IsVisible == true)
                            {
                                WeeklyStepBar.IsVisible = false;
                                WeeklyDistanceBar.IsVisible = true;
                                WeeklyKilocalorieBar.IsVisible = false;
                            }

                            if (StackLayoutMonthlyInform.IsVisible == true)
                            {
                                MonthlyStepBar.IsVisible = false;
                                MonthlyDistanceBar.IsVisible = true;
                                MonthlyKilocalorieBar.IsVisible = false;                               
                            }
                        });

                    }; break;
                case 6:
                {
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            if (StackLayoutWeeklyInform.IsVisible == true)
                            {
                                WeeklyStepBar.IsVisible = false;
                                WeeklyDistanceBar.IsVisible = false;
                                WeeklyKilocalorieBar.IsVisible = true;
                            }

                            if (StackLayoutMonthlyInform.IsVisible == true)
                            {
                                MonthlyStepBar.IsVisible = false;
                                MonthlyDistanceBar.IsVisible = false;
                                MonthlyKilocalorieBar.IsVisible = true;
                            }
                        });

                    }; break;
            }
        }
        
        
        #region Daily Informs

        private void LabelDateChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDailyInform.Focus();
        }

        private void DatePickerDailyInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _reportGoalsDailyViewModel.ClearElementsCollection();
            CalculateInformForDay(DatePickerDailyInform.Date);
        }
        
        private async void CalculateInformForDay(DateTime dateTime)
        {
            List<DailyGoal> dailyGoals = null;
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(_patientId, dateTime);
                _dailyGoalsViewModel.ClearDailyGoalList();
                
                foreach (var item in dailyGoals)
                {
                    _dailyGoalsViewModel.AddDailyGoalToList(item);
                }
            });
            
            var listSteps = dailyGoals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = dailyGoals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = dailyGoals.Where(dg => dg.ActivityId == 3).ToList();

            double maxQuantity = 0;
            double maxProgress = 0;
            
            if (!listSteps.IsEmpty())
            {
                foreach (var item in listSteps)
                {
                    if (item.Progress > maxProgress) maxProgress = item.Progress;
                    if (item.Quantity > maxQuantity) maxQuantity = item.Quantity;
                }

                StepActivity stepActivity = null;

                if (maxProgress == maxQuantity)
                {
                    var listStepRecord = await 
                        StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                    if (listStepRecord.Count > 0)
                    {
                        stepActivity = listStepRecord.First();
                    }
                    
                }
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = stepActivity?.Quantity ?? maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 1
                });
            }
            else
            {
                StepActivity stepActivity = null;
                var listStepRecord = await 
                    StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                if (listStepRecord.Count > 0)
                {
                    stepActivity = listStepRecord.First();
                }

                ////////////////////////////////////////////////

                if (stepActivity != null)
                {
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = stepActivity.Quantity,
                        Quantity = 0,
                        ActivityId = 1
                    });
                }
                else
                {
                    stepActivity = new StepActivity() { Quantity = 0, Total = 0 };
                    
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = stepActivity.Quantity,
                        Quantity = stepActivity.Total,
                        ActivityId = 1
                    });
                }


                

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
                
                KilocalorieActivity kilocalorieActivity = null;

                if (maxProgress == maxQuantity)
                {
                    var listKilocalorieRecord = await 
                        KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                    if (listKilocalorieRecord.Count > 0)
                    {
                        kilocalorieActivity = listKilocalorieRecord.First();
                    }
                    
                }
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = kilocalorieActivity?.Quantity ?? maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 2
                });
            }
            else
            {
                KilocalorieActivity kilocalorieActivity = null;
                
                var listKilocalorieRecord = await 
                    KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                if (listKilocalorieRecord.Count > 0)
                {
                    kilocalorieActivity = listKilocalorieRecord.First();
                }
                
                ////////////////////////////////////////////////

                if (kilocalorieActivity != null)
                {
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = kilocalorieActivity.Quantity,
                        Quantity = 0,
                        ActivityId = 2
                    });
                }
                else
                {
                    kilocalorieActivity = new KilocalorieActivity() { Quantity = 0, Total = 0 };
                    
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = kilocalorieActivity.Quantity,
                        Quantity = kilocalorieActivity.Total,
                        ActivityId = 2
                    });
                    
                }
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
                
                DistanceActivity distanceActivity = null;

                if (maxProgress == maxQuantity)
                {
                    var listDistanceRecord = await 
                        DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                    if (listDistanceRecord.Count > 0)
                    {
                        distanceActivity = listDistanceRecord.First();
                    }
                    
                }
                
                _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = distanceActivity?.Quantity ?? maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }
            else
            {
                DistanceActivity distanceActivity = null;
                
                var listDistanceRecord = await 
                    DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, dateTime, dateTime);

                if (listDistanceRecord.Count > 0)
                {
                    distanceActivity = listDistanceRecord.First();
                }
                
                ////////////////////////////////////////////////

                if (distanceActivity != null)
                {
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = distanceActivity.Quantity,
                        Quantity = 0,
                        ActivityId = 3
                    });
                }
                else
                {
                    distanceActivity = new DistanceActivity() { Quantity = 0, Total = 0 };
                    
                    _reportGoalsDailyViewModel.AddReportGoalToList(new ReportGoal()
                    {
                        Description = "",
                        ImageSource = "",
                        Progress = distanceActivity.Quantity,
                        Quantity = distanceActivity.Total,
                        ActivityId = 3
                    });
                    
                }
            }



            
            
        }

        #endregion
        
        #region Weekly Informs

        /*void InitWeeklyReport()
        {
            var item1 = new ReportGoal()
            {
                Progress = 850,
                Quantity = 950,
                ActivityId = 1,
                ImageSource = "",
                Description = "",
            };

            var item2 = new ReportGoal()
            {
                Progress = 1050,
                Quantity = 1550,
                ActivityId = 2,
                ImageSource = "",
                Description = "",
            };

            var item3 = new ReportGoal()
            {
                Progress = 550,
                Quantity = 750,
                ActivityId = 3,
                ImageSource = "",
                Description = "",
            };
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item1);
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item2);
            
            _reportGoalsWeeklyViewModel.AddReportGoalToList(item3);
            
        }*/
        
        
        private void DatePickerDateWeekStartInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DatePickerDateWeekFinishInform.MinimumDate = DatePickerDateWeekStartInform.Date.AddDays(1);
            _reportGoalsWeeklyViewModel.ClearElementsCollection();
            CalculateInformForWeek(DatePickerDateWeekStartInform.Date,DatePickerDateWeekFinishInform.Date);
        }
        
        private void LabelDate1WeekChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDateWeekStartInform.Focus();
        }
        
        private void DatePickerDateWeekFinishInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DatePickerDateWeekStartInform.MaximumDate = DatePickerDateWeekFinishInform.Date.AddDays(-1);
            _reportGoalsWeeklyViewModel.ClearElementsCollection();
            CalculateInformForWeek(DatePickerDateWeekStartInform.Date,DatePickerDateWeekFinishInform.Date);
        }
        
        private void LabelDate2WeekChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerDateWeekFinishInform.Focus();
        }

        private async void CalculateInformForWeek(DateTime start, DateTime end)
        {
            var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientIdAndDates(_patientId, start, end);

            _weeklyGoalViewModel.ClearWeeklyGoalList();
            foreach (var item in weeklyGoals)
            {
                _weeklyGoalViewModel.AddWeeklyToList(item);
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

                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
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

                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
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

                _reportGoalsWeeklyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = maxProgress,
                    Quantity = maxQuantity,
                    ActivityId = 3
                });
            }


            #region pruebas


            ///////////// PRUEBAS  ///////////////////////

            //var listdailyGoalPerWeek = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDates(_patientId, start, end);
            //prueba1.Text = listdailyGoalPerWeek.Count.ToString();

            //var weeklylistSteps = listdailyGoalPerWeek.Where(dg => dg.ActivityId == 1).ToList();
            //prueba2.Text = weeklylistSteps.Count.ToString();

            _listStepActivities.Clear();


            var listStepRecord = await
                    StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start, end);

            if (listStepRecord != null)
            {
                /*foreach(var step in listStepRecord)
                {
                    if(step.Quantity == 0)
                    {
                        _listStepActivities.Add(new StepActivity()
                        {
                            Id = 0,
                            Quantity = 0,
                            Date = start,
                            Total = 1,
                            PatientId = 1,
                            Percentage = 1,
                        });
                    } else
                    {
                        _listStepActivities.Add(new StepActivity()
                        {
                            Id = 0,
                            Quantity = 1,
                            Date = start,
                            Total = 1,
                            PatientId = 1,
                            Percentage = 1,
                        });
                    }
                    
                                   
                    //prueba8.Text = step.Quantity.ToString();
                    
                    //Console.WriteLine(_listStepActivities[0].Quantity.ToString());
                    
                }*/
            }


            //prueba 20 XD

            var _chartEntries = new List<ChartEntry>();

            List<StepActivity> listSteps1 = new List<StepActivity>();

            StepActivity stepActivityLunes = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivityMartes = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivityMiercoles = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivityueves = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivityViernes = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivitySabado = new StepActivity()
            {
                Quantity = 0,
            };
            StepActivity stepActivityDomingo = new StepActivity()
            {
                Quantity = 0,
            };

            List<double> listadoublesSteps = new List<double>();
            var listStepRecordLunes = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start, start);

            var listStepRecordMartes = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(1), start.AddDays(1));

            var listStepRecordMiercoles = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(2), start.AddDays(2));

            var listStepRecordJueves = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(3), start.AddDays(3));

            var listStepRecordViernes = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(4), start.AddDays(4));

            var listStepRecordSabado = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(5), start.AddDays(5));

            var listStepRecordDomingo = await
                StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, end, end);

            // Lunes
            if (listStepRecordLunes.Count > 0)
            {
                stepActivityLunes = listStepRecordLunes.First();
                var entriesStep = new ChartEntry(Convert.ToSingle(stepActivityLunes.Quantity))
                {
                    ValueLabel = stepActivityLunes.Quantity.ToString(),
                    Label = "Lunes",
                };
                _chartEntries.Add(entriesStep);
            }
            

            // Martes
            if (listStepRecordMartes.Count > 0)
            {
                stepActivityMartes = listStepRecordMartes.First();
                var entriesStep2 = new ChartEntry(Convert.ToSingle(stepActivityMartes.Quantity))
                {
                    ValueLabel = stepActivityMartes.Quantity.ToString(),
                    Label = "M",
                };
                _chartEntries.Add(entriesStep2);
            }
            

            // Miercoles
            if (listStepRecordMiercoles.Count > 0)
            {
                stepActivityMiercoles = listStepRecordMiercoles.First();
                var entriesStep3 = new ChartEntry(Convert.ToSingle(stepActivityMiercoles.Quantity))
                {
                    ValueLabel = stepActivityMiercoles.Quantity.ToString(),
                    Label = "X",
                };
                _chartEntries.Add(entriesStep3);
            }
            

            // Jueves
            if (listStepRecordJueves.Count > 0)
            {
                stepActivityueves = listStepRecordJueves.First();
                var entriesStep4 = new ChartEntry(Convert.ToSingle(stepActivityueves.Quantity))
                {
                    ValueLabel = stepActivityueves.Quantity.ToString(),
                    Label = "J",
                };
                _chartEntries.Add(entriesStep4);
            }
            

            // Viernes
            if (listStepRecordViernes.Count > 0)
            {
                stepActivityViernes = listStepRecordViernes.First();
                var entriesStep5 = new ChartEntry(Convert.ToSingle(stepActivityViernes.Quantity))
                {
                    ValueLabel = stepActivityViernes.Quantity.ToString(),
                    Label = "V",
                };
                _chartEntries.Add(entriesStep5);
            }
            

            // Sabado
            if (listStepRecordSabado.Count > 0)
            {
                stepActivitySabado = listStepRecordSabado.First();
                var entriesStep6 = new ChartEntry(Convert.ToSingle(stepActivitySabado.Quantity))
                {
                    ValueLabel = stepActivitySabado.Quantity.ToString(),
                    Label = "S",
                    Color=SKColors.Orange,
                };
                _chartEntries.Add(entriesStep6);
            }
            

            // Domingo
            if (listStepRecordDomingo.Count > 0)
            {
                stepActivityDomingo = listStepRecordDomingo.First();
                var entriesStep7 = new ChartEntry(Convert.ToSingle(stepActivityDomingo.Quantity))
                {
                    ValueLabel = stepActivityDomingo.Quantity.ToString(),
                    Label = "D",
                };
                _chartEntries.Add(entriesStep7);
            }
            

            /*prueba1.Text = stepActivityLunes.Quantity.ToString();
            prueba2.Text = stepActivityLunes.Date.ToString();
            prueba3.Text = stepActivityMartes.Quantity.ToString();
            prueba4.Text = stepActivityMartes.Date.ToString();
            prueba5.Text = stepActivityMiercoles.Quantity.ToString();
            //prueba5.Text = stepActivityueves.Date.ToString();
            prueba6.Text = stepActivityueves.Quantity.ToString();
            prueba7.Text = stepActivityViernes.Quantity.ToString();
            prueba8.Text = stepActivitySabado.Quantity.ToString();
            prueba9.Text = stepActivityDomingo.Quantity.ToString();*/

            /*DailyStepBarData.Chart = new BarChart()
            {
                Entries = _chartEntries,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            DailyDistanceBarData.Chart = new BarChart()
            {
                Entries = _chartEntries,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            DailyKilocalorieBarData.Chart = new BarChart()
            {
                Entries = _chartEntries,
                LabelTextSize = 25,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };*/

            #endregion

            GetDataWeekly(1, start, end);
            GetDataWeekly(2, start, end);
            GetDataWeekly(3, start, end);

        }

        #endregion

        #region Monthly Inform



        private async void CalculateInformForMonth(DateTime dateTime)
        {
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //var goals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDates(_patient.Id,firstDayOfMonth,lastDayOfMonth);
            
            
            /*var listSteps = goals.Where(dg => dg.ActivityId == 1).ToList();
            var listCalories = goals.Where(dg => dg.ActivityId == 2).ToList();
            var listDistances = goals.Where(dg => dg.ActivityId == 3).ToList();*/

            List<StepActivity> listSteps = new List<StepActivity>();
            List<KilocalorieActivity> listCalories= new List<KilocalorieActivity>();
            List<DistanceActivity> listDistances= new List<DistanceActivity>();

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                listSteps = await 
                    StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);
            
                listCalories = await 
                    KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);
            
                listDistances = await 
                    DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);
            });
            
            

            
            if (!listSteps.IsEmpty())
            {
                double total = 0.0;
                foreach (var item in listSteps)
                {
                    total += item.Quantity;
                }
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = total,
                    Quantity = 0,
                    ActivityId = 1
                });
            }
                     
            if (!listCalories.IsEmpty())
            {
                double total = 0.0;
                foreach (var item in listCalories)
                {
                    total += item.Quantity;
                }
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = total,
                    Quantity = 0,
                    ActivityId = 2
                });
            }
            
            if (!listDistances.IsEmpty())
            {
                double total = 0.0;
                foreach (var item in listDistances)
                {
                    total += item.Quantity;
                }
                
                _reportGoalsMonthlyViewModel.AddReportGoalToList(new ReportGoal()
                {
                    Description = "",
                    ImageSource = "",
                    Progress = total,
                    Quantity = 0,
                    ActivityId = 3
                });
            }

            GetDataMonthly(dateTime);
        }
        
        private void DatePickerMonthInform_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _reportGoalsMonthlyViewModel.ClearElementsCollection();
            CalculateInformForMonth(DatePickerMonthInform.Date);
        }
        
        private void LabelMonthChevron_OnTapped(object sender, EventArgs e)
        {
            DatePickerMonthInform.Focus();
        }



        #endregion

        
        private async void GetDataWeekly(int activityId ,DateTime start, DateTime end)
        {
            var _chartEntriesStepActivity = new List<ChartEntry>();
            var _chartEntriesDistanceActivity = new List<ChartEntry>();
            var _chartEntriesKilocalorieActivity = new List<ChartEntry>();

            string ActivityDay = null;

            StepActivity StepActivityDay = new StepActivity();
            DistanceActivity DistanceActivityDay = new DistanceActivity();
            KilocalorieActivity KilocalorieActivityDay = new KilocalorieActivity();


            for (int i = 0 ; i < 7 ; i++)
            {
                if (i == 0) ActivityDay = "L";
                if (i == 1) ActivityDay = "M";
                if (i == 2) ActivityDay = "X";
                if (i == 3) ActivityDay = "J";
                if (i == 4) ActivityDay = "V";
                if (i == 5) ActivityDay = "S";
                if (i == 6) ActivityDay = "D";

                // StepsActivity
                if (activityId == 1)
                {
                    var listStepRecordDay = await
                        StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, start.AddDays(i), start.AddDays(i));

                    if (listStepRecordDay.Count > 0)
                    {
                        StepActivityDay = listStepRecordDay.First();
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

                    WeeklyStepBarData.Chart = new BarChart()
                    {
                        Entries = _chartEntriesStepActivity,
                        LabelTextSize = 25,
                        BarAreaAlpha = 0,
                        BackgroundColor = SKColor.Parse("#F0F3F4"),
                        LabelOrientation = Orientation.Horizontal,
                        ValueLabelOrientation = Orientation.Horizontal,
                    };
                }

                // DistancesActivity
                if (activityId == 2)
                {
                    var listDistanceRecordDay = await
                        DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, start.AddDays(i), start.AddDays(i));

                    if (listDistanceRecordDay.Count > 0)
                    {
                        DistanceActivityDay = listDistanceRecordDay.First();
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

                    WeeklyDistanceBarData.Chart = new BarChart()
                    {
                        Entries = _chartEntriesDistanceActivity,
                        LabelTextSize = 25,
                        BarAreaAlpha = 0,
                        BackgroundColor = SKColor.Parse("#F0F3F4"),
                        LabelOrientation = Orientation.Horizontal,
                        ValueLabelOrientation = Orientation.Horizontal,
                    };
                }

                // KilocaloriesActivity
                if (activityId == 3)
                {                    
                    var listKilocalorieRecordDay = await
                        KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, start.AddDays(i), start.AddDays(i));

                    if (listKilocalorieRecordDay.Count > 0)
                    {
                        KilocalorieActivityDay = listKilocalorieRecordDay.First();
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

        private async void GetDataMonthly(DateTime dateTime)
        {
            var _chartEntriesStepActivityMonth = new List<ChartEntry>();
            var _chartEntriesKilocalorieActivityMonth = new List<ChartEntry>();
            var _chartEntriesDistanceActivityMonth = new List<ChartEntry>();

            List<StepActivity> listSteps = new List<StepActivity>();
            List<KilocalorieActivity> listCalories = new List<KilocalorieActivity>();
            List<DistanceActivity> listDistances = new List<DistanceActivity>();

            string ActivityMonth = null;

            for (int i = 1; i <= 12; i++)
            {
                var firstMonthOfYear = new DateTime(dateTime.Year, i, 1);

                var firstDayOfMonth = new DateTime(firstMonthOfYear.Year, firstMonthOfYear.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                await Device.InvokeOnMainThreadAsync(async () =>
                {
                    listSteps = await
                        StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);

                    listCalories = await
                        KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);

                    listDistances = await
                        DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, firstDayOfMonth, lastDayOfMonth);
                });

                if (i == 1) ActivityMonth = "Ene";
                if (i == 2) ActivityMonth = "Feb";
                if (i == 3) ActivityMonth = "Mar";
                if (i == 4) ActivityMonth = "Abr";
                if (i == 5) ActivityMonth = "May";
                if (i == 6) ActivityMonth = "Jun";
                if (i == 7) ActivityMonth = "Jul";
                if (i == 8) ActivityMonth = "Ago";
                if (i == 9) ActivityMonth = "Set";
                if (i == 10) ActivityMonth = "Oct";
                if (i == 11) ActivityMonth = "Nov";
                if (i == 12) ActivityMonth = "Dic";

                // Steps per Month
                if (!listSteps.IsEmpty())
                {
                    double total = 0.0;
                    foreach (var item in listSteps)
                    {
                        total += item.Quantity;
                    }

                    var entriesStep = new ChartEntry(Convert.ToSingle(total))
                    {
                        ValueLabel = total.ToString(),
                        Label = ActivityMonth,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesStepActivityMonth.Add(entriesStep);
                }
                else
                {
                    var entriesStep = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityMonth,
                        Color = SKColors.Black,
                    };
                    _chartEntriesStepActivityMonth.Add(entriesStep);
                }

                // Kilocalorie per Month
                if (!listCalories.IsEmpty())
                {
                    double total = 0.0;
                    foreach (var item in listCalories)
                    {
                        total += item.Quantity;
                    }
                    var entriesKilocalorie = new ChartEntry(Convert.ToSingle(total))
                    {
                        ValueLabel = total.ToString(),
                        Label = ActivityMonth,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesKilocalorieActivityMonth.Add(entriesKilocalorie);
                }
                else
                {
                    var entriesKilocalorie = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityMonth,
                        Color = SKColors.Black,
                    };
                    _chartEntriesKilocalorieActivityMonth.Add(entriesKilocalorie);
                }

                // Distance per Month
                if (!listDistances.IsEmpty())
                {
                    double total = 0.0;
                    foreach (var item in listDistances)
                    {
                        total += item.Quantity;
                    }
                    var entriesDistance = new ChartEntry(Convert.ToSingle(total))
                    {
                        ValueLabel = total.ToString(),
                        Label = ActivityMonth,
                        Color = SKColors.Orange,
                    };
                    _chartEntriesDistanceActivityMonth.Add(entriesDistance);
                }
                else
                {
                    var entriesDistance = new ChartEntry(0)
                    {
                        ValueLabel = "0",
                        Label = ActivityMonth,
                        Color = SKColors.Black,
                    };
                    _chartEntriesDistanceActivityMonth.Add(entriesDistance);
                }
            }
   
            MonthlyStepBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesStepActivityMonth,
                LabelTextSize = 20,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            MonthlyKilocalorieBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesKilocalorieActivityMonth,
                LabelTextSize = 20,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };

            MonthlyDistanceBarData.Chart = new BarChart()
            {
                Entries = _chartEntriesDistanceActivityMonth,
                LabelTextSize = 18,
                BarAreaAlpha = 0,
                BackgroundColor = SKColor.Parse("#F0F3F4"),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };
        }
    }
}