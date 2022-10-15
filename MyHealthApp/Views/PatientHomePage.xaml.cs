using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Helpers;
using MyHealthApp.Models;
using MyHealthApp.Models.Activities;
using MyHealthApp.Services;
using MyHealthApp.Services.Activities;
using MyHealthApp.Services.MiBand;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.Register;
using MyHealthApp.Views.Report;
using ProgressRingControl.Forms.Plugin;
using WindesHeartSDK;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyHealthApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientHomePage : ContentPage
    {
        private readonly PatientDailyGoalsViewModel _dailyGoalsViewModel;
        private readonly PatientWeeklyGoalViewModel _weeklyGoalViewModel;
        private readonly ValueToDoubleConverter _valueToDoubleConverter = new ValueToDoubleConverter();

        private readonly TimeSpan _second = TimeSpan.FromSeconds(3);
        private readonly StepsViewModel _stepsViewModel;
        private bool _isTimerWorking = false;
        private bool _isCheckingSteps = false;
        private readonly long _patientId;

        //Pasos
        private DailyGoal _firstStepDg;
        private StepActivity _stepActivity;//Osea el general que va a registrar siempre pasos
        
        //Distancia
        private DailyGoal _firstDistanceDg;
        private DistanceActivity _distanceActivity;
        
        //Kilocalorias
        private DailyGoal _firstKilocalorieDg;
        private KilocalorieActivity _kilocalorieActivity;

        private WeeklyGoal _firstStepWeeklyGoal;
        private Double _firstStepWgProgressAux = 0.0;

        

        private WeeklyGoal _firstDistanceWeeklyGoal;
        private WeeklyGoal _firstKilocalorieWeeklyGoal;

        private bool _IsDisplayedNoInternetMsg = false;

        public PatientHomePage()
        {
            /*if (SuccessfulRegisterPage.DailyGoals != null && SuccessfulRegisterPage.DailyGoals != null)
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(SuccessfulRegisterPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(SuccessfulRegisterPage.WeeklyGoals);
            }
            else
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(LoginPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(LoginPage.WeeklyGoals);
            }*/
            InitializeComponent();

            _stepsViewModel = new StepsViewModel();
            _dailyGoalsViewModel = new PatientDailyGoalsViewModel();
            _weeklyGoalViewModel = new PatientWeeklyGoalViewModel();


            _patientId = long.Parse(Application.Current.Properties["PatientId"].ToString());

            GetDailyGoalsAndWeeklyGoals();
        }


        private async void GetDailyGoalsAndWeeklyGoals()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                //Los dailyGoals
                FrameDailyGoals.BindingContext = _dailyGoalsViewModel;

                //Los weeklyGoals
                FrameWeeklyGoals.BindingContext = _weeklyGoalViewModel;
                
                await GetCurrentStepRecord();
                await GetCurrentDistanceRecord();
                await GetCurrentKilocalorieRecord();

                var dailyGoals =
                    await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(_patientId, DateTime.Today);
                var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(_patientId);

                foreach (var item in dailyGoals)
                {
                    _dailyGoalsViewModel.AddDailyGoalToList(item);
                }

                foreach (var item in weeklyGoals)
                {
                    _weeklyGoalViewModel.AddWeeklyToList(item);
                }
                
                
            });


            Device.BeginInvokeOnMainThread(() =>
            {
                FlexLayoutDailyGoals.IsVisible = true;
                FlexLayoutWeeklyGoals.IsVisible = true;
                
                //Step
                
                GetDailyGoalStep();

                //Distance
                
                GetDailyGoalDistance();
                
                //Kilocalorie
                
                GetDailyGoalKilocalorie();
                
                
                GetGoalsInformation();
                //-----------------------------
                //GetWeeklyGoalStep();
            });
        }

        private async Task GetCurrentStepRecord()
        {
            var stepActivitiesList =
                await StepService.Instance.GetStepActivitiesByPatientIdAndDates(_patientId, DateTime.Today, DateTime.Today);
                
            if (stepActivitiesList.Count == 0)
            {
                //Si es cero entonces aun no hay un registro para los pasos de hoy
                    
                //Lo creamos
                var createdStepActivity = await 
                    StepService.Instance.PostStepActivityByPatientId(_patientId, new StepActivity() { Quantity = 0,Date = DateTime.Today});

                _stepActivity = createdStepActivity;
                _stepActivity.Total = 99999;
                _stepActivity.Percentage = 0.0;
            }
            else
            {
                //Si no es cero, hay un elemento creado para el registro de hoy de pasos

                _stepActivity = stepActivitiesList.First();
                _stepActivity.Total = 99999;
                _stepActivity.Percentage = 0.0;
            }
            
            
        }
        private void GetDailyGoalStep()
        {
            try
            {
                _firstStepDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity)
                    .ToList().First();

                LabelProgressSteps.BindingContext = _firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty, "Progress");

                LabelGoalSteps.BindingContext = _firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default, stringFormat: "/{0}");


                ProgressRingSteps.BindingContext = _firstStepDg;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);

                if (_firstStepDg.Progress == 0.0)
                {
                    if(_stepsViewModel.TodayStepCount > _firstStepDg.Quantity)
                    {
                        _firstStepDg.Progress = _firstStepDg.Quantity;
                    }
                    /*else
                    {
                        _firstStepDg.Progress = _stepsViewModel.TodayStepCount;
                        await DailyGoalService.Instance.PutDailyGoalStepByPatientId(_patientId, _firstStepDg);
                    }*/
                    
                }

                //LabelProgressSteps.Text = firstStepDg.Progress.ToString(CultureInfo.CurrentCulture);
                //LabelGoalSteps.Text = "/"+firstStepDg.Quantity.ToString(CultureInfo.CurrentCulture);
                //ProgressRingSteps.Progress = firstStepDg.Percentage;
            }
            catch (InvalidOperationException e)
            {
                LabelGoalSteps.TextColor = Color.White;

                
                _stepActivity.CalculatePercentage();
                
                
                LabelProgressSteps.BindingContext = _stepActivity;
                LabelProgressSteps.SetBinding(Label.TextProperty, "Quantity");

                LabelGoalSteps.BindingContext = _stepActivity;
                LabelGoalSteps.SetBinding(Label.TextProperty, "Total", BindingMode.Default);


                ProgressRingSteps.BindingContext = _stepActivity;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);


                _firstStepDg = null;



                //_stepActivity = new StepActivity()


                /*_firstStepDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 99999.0,
                    Progress = _stepsViewModel.TodayStepCount,
                    ActivityId = 1,
                };

                LabelProgressSteps.BindingContext = _firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty, "Progress");

                LabelGoalSteps.BindingContext = _firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default);


                ProgressRingSteps.BindingContext = _firstStepDg;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);*/


                /*if (firstStepDg != null)
                {
                    firstStepDg.Id = -1;
                    firstStepDg.Percentage = 0;
                    firstStepDg.Quantity = 9999.0;
                    firstStepDg.Progress = 0;
                    firstStepDg.ActivityId = 1;
                }
                else
                {
                    firstStepDg = new DailyGoal()
                    {
                        Id = -1,
                        Percentage = 0,
                        Quantity = 9999.0,
                        Progress = 0,
                        ActivityId = 1,
                    };
                    
                    LabelProgressSteps.BindingContext = firstStepDg;
                    LabelProgressSteps.SetBinding(Label.TextProperty,"Progress");

                    LabelGoalSteps.BindingContext = firstStepDg;
                    LabelGoalSteps.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                    ValueToDoubleConverter valueToDoubleConverter = new ValueToDoubleConverter();
                    ProgressRingSteps.BindingContext = firstStepDg;
                    ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,valueToDoubleConverter);
                    
                }*/


                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingSteps);
                //FlexLayoutRingsToday.Children.Remove(ProgressRingSteps);
                //FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                //FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
            }
        }

        private void GetWeeklyGoalStep()
        {
            int leftStepWeeklyGoalsCount =
                _weeklyGoalViewModel.WeeklyGoals.Count(e => e.ActivityId == 1 && e.Progress < e.Quantity);

            if (leftStepWeeklyGoalsCount > 0)
            {
                _firstStepWeeklyGoal = _weeklyGoalViewModel.WeeklyGoals
                    .Where(e => e.ActivityId == 1 && e.Progress < e.Quantity)
                    .ToList().First();

                
                //Esto solo funciona en el mismo dia, no estoy actualizando el progressAux
                if (_firstStepWeeklyGoal.Progress == 0.0)
                {
                    //_firstStepWgProgressAux = 0;
                    _firstStepWeeklyGoal.Progress = _firstStepWgProgressAux;
                    _firstStepWgProgressAux = 0;
                    //Respaldamos el progreso

                }
                else
                {
                    //Si nos reconectamos el mismo dia
                    _firstStepWgProgressAux = 0;
                    
                    //Si nos conectamos un dia distinto
                    _firstStepWgProgressAux = _firstStepWeeklyGoal.Progress;
                }
                

            }
            else
            {
                _firstStepWeeklyGoal = null;
            }
        }

        private async Task GetCurrentDistanceRecord()
        {
            var distanceActivitiesList =
                await DistanceService.Instance.GetDistanceActivitiesByPatientIdAndDates(_patientId, DateTime.Today, DateTime.Today);
                
            if (distanceActivitiesList.Count == 0)
            {
                //Si es cero entonces aun no hay un registro para la distancia de hoy
                    
                //Lo creamos
                var createdDistanceActivity = await 
                    DistanceService.Instance.PostDistanceActivityByPatientId(_patientId, new DistanceActivity() { Quantity = 0,Date = DateTime.Today});
                /*var stepActivityExist =
                    await StepService.Instance.GetStepActivityByIdAndPatientIdAndDate(createdStepActivity.Id, _patientId, DateTime.Today);*/

                _distanceActivity = createdDistanceActivity;
                _distanceActivity.Total = 99999;
                _distanceActivity.Percentage = 0.0;
            }
            else
            {
                //Si no es cero, hay un elemento creado para el registro de hoy de distancia

                _distanceActivity = distanceActivitiesList.First();
                _distanceActivity.Total = 99999;
                _distanceActivity.Percentage = 0.0;
            }
        }

        private void GetDailyGoalDistance()
        {
            try
            {
                _firstDistanceDg = _dailyGoalsViewModel.DailyGoals
                    .Where(e => e.ActivityId == 3 && e.Progress < e.Quantity).ToList().First();

                LabelProgressDistance.BindingContext = _firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty, "Progress", stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = _firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default,
                    stringFormat: "{0:#.0} m");


                ProgressRingDistance.BindingContext = _firstDistanceDg;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);

                //LabelProgressDistance.Text = firstDistanceDg.Progress.ToString(CultureInfo.CurrentCulture)+" m";
                //LabelGoalDistance.Text = "/"+firstDistanceDg.Quantity.ToString(CultureInfo.CurrentCulture)+" m";
                //ProgressRingDistance.Progress = firstDistanceDg.Percentage;

                if (_firstDistanceDg.Progress == 0.0)
                {
                    if((_stepsViewModel.TodayStepCount * 0.62) > _firstDistanceDg.Quantity)
                    {
                        _firstDistanceDg.Progress = _firstDistanceDg.Quantity;
                    }
                    /*else
                    {
                        _firstDistanceDg.Progress = _stepsViewModel.TodayStepCount * 0.66;
                        await DailyGoalService.Instance.PutDailyGoalDistanceByPatientId(_patientId, _firstDistanceDg);
                    }*/


                    
                }
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingDistance);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingDistance);

                LabelGoalDistance.TextColor = Color.White;
                
                
                _distanceActivity.CalculatePercentage();
                
                
                LabelProgressDistance.BindingContext = _distanceActivity;
                LabelProgressDistance.SetBinding(Label.TextProperty, "Quantity", stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = _distanceActivity;
                LabelGoalDistance.SetBinding(Label.TextProperty, "Total", BindingMode.Default);


                ProgressRingDistance.BindingContext = _distanceActivity;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);

                _firstDistanceDg = null;

                /*_firstDistanceDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 99999.0,
                    Progress = _stepsViewModel.TodayStepCount * 0.62, //Converting Steps to meters
                    ActivityId = 3,
                };

                LabelProgressDistance.BindingContext = _firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty, "Progress", stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = _firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default);


                ProgressRingDistance.BindingContext = _firstDistanceDg;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);*/


                //FlexLayoutRingsToday.JustifyContent = FlexJustify.Center;
                //FlexLayoutRingsInfoToday.JustifyContent = FlexJustify.Center;
            }

            /*if (FlexLayoutRingsToday.Children.Count == 0)
            {
                FlexLayoutRingsToday.Children.Add(new Label()
                {
                    Text = "No hay objetivos diarios o pendientes",
                    FontFamily = "ArchivoRegular",
                    Padding = new Thickness(0,20,0,50),
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 14
                });
            }*/
        }

        private async Task GetCurrentKilocalorieRecord()
        {
            var kilocalorieActivitiesList =
                await KilocalorieService.Instance.GetKilocalorieActivitiesByPatientIdAndDates(_patientId, DateTime.Today, DateTime.Today);
                
            if (kilocalorieActivitiesList.Count == 0)
            {
                //Si es cero entonces aun no hay un registro para la distancia de hoy
                    
                //Lo creamos
                var createdKilocalorieActivity = await 
                    KilocalorieService.Instance.PostKilocalorieActivityByPatientId(_patientId, new KilocalorieActivity() { Quantity = 0,Date = DateTime.Today});
                /*var stepActivityExist =
                    await StepService.Instance.GetStepActivityByIdAndPatientIdAndDate(createdStepActivity.Id, _patientId, DateTime.Today);*/

                _kilocalorieActivity = createdKilocalorieActivity;
                _kilocalorieActivity.Total = 99999;
                _kilocalorieActivity.Percentage = 0.0;
            }
            else
            {
                //Si no es cero, hay un elemento creado para el registro de hoy de distancia

                _kilocalorieActivity = kilocalorieActivitiesList.First();
                _kilocalorieActivity.Total = 99999;
                _kilocalorieActivity.Percentage = 0.0;
            }
        }

        private void GetDailyGoalKilocalorie()
        {
            try
            {
                _firstKilocalorieDg = _dailyGoalsViewModel.DailyGoals
                    .Where(e => e.ActivityId == 2 && e.Progress < e.Quantity).ToList().First();

                LabelProgresskilocalorie.BindingContext = _firstKilocalorieDg;
                LabelProgresskilocalorie.SetBinding(Label.TextProperty, "Progress", stringFormat: "{0} kcal");

                LabelGoalkilocalorie.BindingContext = _firstKilocalorieDg;
                LabelGoalkilocalorie.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default,
                    stringFormat: "{0} kcal");


                ProgressRingKilocalorie.BindingContext = _firstKilocalorieDg;
                ProgressRingKilocalorie.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);

                //LabelProgressDistance.Text = firstDistanceDg.Progress.ToString(CultureInfo.CurrentCulture)+" m";
                //LabelGoalDistance.Text = "/"+firstDistanceDg.Quantity.ToString(CultureInfo.CurrentCulture)+" m";
                //ProgressRingDistance.Progress = firstDistanceDg.Percentage;

                if (_firstKilocalorieDg.Progress == 0.0)
                {
                    if((_stepsViewModel.TodayStepCount * 0.018) > _firstKilocalorieDg.Quantity)
                    {
                        _firstKilocalorieDg.Progress = _firstKilocalorieDg.Quantity;
                    }
                    /*else
                    {
                        _firstKilocalorieDg.Progress = _stepsViewModel.TodayStepCount * 0.02;
                        await DailyGoalService.Instance.PutDailyGoalKilocalorieByPatientId(_patientId, _firstKilocalorieDg);
                    }*/
                    
                }
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingDistance);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingDistance);
                
                LabelGoalkilocalorie.TextColor = Color.White;
                
                
                _kilocalorieActivity.CalculatePercentage();
                
                
                LabelProgresskilocalorie.BindingContext = _kilocalorieActivity;
                LabelProgresskilocalorie.SetBinding(Label.TextProperty, "Quantity", stringFormat: "{0:#.0} m");

                LabelGoalkilocalorie.BindingContext = _kilocalorieActivity;
                LabelGoalkilocalorie.SetBinding(Label.TextProperty, "Total", BindingMode.Default);


                ProgressRingKilocalorie.BindingContext = _kilocalorieActivity;
                ProgressRingKilocalorie.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);

                
                _firstKilocalorieDg = null;

                /*LabelGoalkilocalorie.TextColor = Color.White;

                _firstKilocalorieDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 99999.0,
                    Progress = _stepsViewModel.TodayStepCount * 0.018, //Converting Steps to calories
                    ActivityId = 2,
                };

                LabelProgresskilocalorie.BindingContext = _firstKilocalorieDg;
                LabelProgresskilocalorie.SetBinding(Label.TextProperty, "Progress", stringFormat: "{0} kcal");

                LabelGoalkilocalorie.BindingContext = _firstKilocalorieDg;
                LabelGoalkilocalorie.SetBinding(Label.TextProperty, "Quantity", BindingMode.Default);


                ProgressRingKilocalorie.BindingContext = _firstKilocalorieDg;
                ProgressRingKilocalorie.SetBinding(ProgressRing.ProgressProperty, "Percentage", BindingMode.Default,
                    _valueToDoubleConverter);*/
            }
        }

        private void GetGoalsInformation()
        {
            UpdateCompletedDailyGoals();
            UpdateCompletedWeeklyGoals();


            //BindableLayout.SetItemsSource(FlexLayoutDailyGoals,_dailyGoalsViewModel.DailyGoals);
            //Debug.WriteLine("Tengo hijos: " + FlexLayoutDailyGoals.Children.Count);
            //FlexLayoutDailyGoals.SetBinding(BindableLayout.ItemsSourceProperty,"DailyGoals");


            //BindableLayout.SetItemsSource(FlexLayoutWeeklyGoals,_weeklyGoalViewModel.WeeklyGoals);
            //Debug.WriteLine("Tengo hijos: " + FlexLayoutWeeklyGoals.Children.Count);
        }


        protected override void OnAppearing()
        {
            //App.RequestLocationPermission();
            //if (Windesheart.PairedDevice == null)
            //return;


            if (Windesheart.PairedDevice != null && Windesheart.PairedDevice.IsAuthenticated())
            {
                ButtonConnectSmartWatch.IsVisible = false;
                _isTimerWorking = true;
                SetupDailyGoal();
                if (_firstStepDg != null)
                {
                    Windesheart.PairedDevice?.SetStepGoal(int.Parse(_firstStepDg.Quantity.ToString()));
                }
                else
                {
                    Windesheart.PairedDevice?.SetStepGoal(int.Parse(_stepActivity.Total.ToString()));
                }
                
            }
            else
            {
                ButtonConnectSmartWatch.IsVisible = true;
                //HandleAutoConnect();
            }
        }

        protected override void OnDisappearing()
        {
            _isTimerWorking = false;
        }

        private async void UpdatingStepDg()
        {
            //Es decir su id existe en la base de datos
            if (_firstStepDg != null)
            {
                //_stepsViewModel.UpdateInfo();
                Debug.Print("Contando..." + _stepsViewModel.TodayStepCount.ToString() + " pasos");

                if (_firstStepDg.Progress != (double)_stepsViewModel.TodayStepCount
                    && _stepsViewModel.TodayStepCount != 0)
                {
                    //Si mi contador supero mi objetivo
                    if ((double)_stepsViewModel.TodayStepCount >= _firstStepDg.Quantity)
                    {
                        _firstStepDg.Progress = (double)_firstStepDg.Quantity;
                        _firstStepDg.CalculatePercentage();


                        var resp = await DailyGoalService.Instance.PutDailyGoalStepByPatientId(_patientId, _firstStepDg);


                        //Al finalizar el objetivo obtenemos el siguiente
                        //Si no hay se inventa localmente uno para rastrear tu avance
                        if(resp != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Actualizamo UI importante
                                //Cuando estamos dentro de un timer
                                UpdateCompletedDailyGoals();
                                GetDailyGoalStep();

                                //Actualizamos su porcentaje de avance del nuevo objetivo de la cola

                                if (_firstStepDg != null)
                                {
                                    _firstStepDg.CalculatePercentage();
                                    //Seteamos el nuevo objetivo
                                    Windesheart.PairedDevice.SetStepGoal(int.Parse(_firstStepDg.Quantity.ToString()));
                                }
                                else
                                {
                                    Windesheart.PairedDevice.SetStepGoal(int.Parse(_stepActivity.Total.ToString()));
                                }
                                
                            });
                        }

                        
                    }
                    //Si estoy debajo de mi objetivo
                    else
                    {
                        /*_firstStepDg.Progress = (double)_stepsViewModel.TodayStepCount;
                        _firstStepDg.CalculatePercentage();


                        await DailyGoalService.Instance.PutDailyGoalStepByPatientId(_patientId, _firstStepDg);*/

                        foreach (var item in _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity).ToList())
                        {
                            item.Progress = (double)_stepsViewModel.TodayStepCount;
                            item.CalculatePercentage();
                            await DailyGoalService.Instance.PutDailyGoalStepByPatientId(_patientId,
                                item);
                        }


                    }
                }
            }
            else
            {
                Debug.Print("(Registrando para hoy) Contando..." + _stepsViewModel.TodayStepCount.ToString() + " pasos");
                
            }
            
            //Siempre vamos a estar registrando los pasos
            if (_stepActivity.Quantity != (double)_stepsViewModel.TodayStepCount)
            {
                _stepActivity.Quantity = (double)_stepsViewModel.TodayStepCount;
                _stepActivity.CalculatePercentage();

                await StepService.Instance.UpdateStepActivityByPatientIdAndId(_patientId,_stepActivity.Id,_stepActivity);
            };
            
            
            
            
        }

        private void UpdatingStepWg()
        {
            if (_firstStepWeeklyGoal != null)
            {
                
                //_stepsViewModel.UpdateInfo();
                Debug.Print("Contando del semanal..." + (_stepsViewModel.TodayStepCount + _firstStepWgProgressAux).ToString() + " pasos");

                if (_stepsViewModel.TodayStepCount == 0) _firstStepWgProgressAux = 0;
                if (_firstStepWeeklyGoal.Progress != ((double)_stepsViewModel.TodayStepCount+_firstStepWgProgressAux)
                    && _stepsViewModel.TodayStepCount != 0)
                {
                    //Si mi contador + progreso actual supero mi objetivo
                    if (((double)_stepsViewModel.TodayStepCount+_firstStepWgProgressAux) > _firstStepWeeklyGoal.Quantity)
                    {
                        _firstStepWeeklyGoal.Progress = (double)_firstStepDg.Quantity;
                        _firstStepWeeklyGoal.CalculatePercentage();


                        Task.Run(async () =>
                        {
                            await WeeklyGoalService.Instance.PutWeeklyGoalByPatientId(_patientId, _firstStepWeeklyGoal);


                            //Al finalizar el objetivo obtenemos el siguiente

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Actualizamo UI importante
                                //Cuando estamos dentro de un timer
                                UpdateCompletedWeeklyGoals();
                                _firstStepWgProgressAux = _firstStepWeeklyGoal.Progress;
                                GetWeeklyGoalStep();

                                if (_firstStepWeeklyGoal != null)//Es null si ya no hay weeklygoal en cola
                                {
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola

                                    _firstStepWeeklyGoal.CalculatePercentage();
                                
                                    //Seteamos el nuevo objetivo, -1 es el dailyGoal pasos si se acabaron los de su cola y actividad
                                    if (_firstStepDg.Id == -1)
                                    {
                                        Windesheart.PairedDevice.SetStepGoal(int.Parse(_firstStepWeeklyGoal.Quantity.ToString()));
                                    }
                                }
                                
                                
                            });

                            
                        });
                    }
                    //Si el contador + progreso esta debajo de mi objetivo 
                    else
                    {
                        _firstStepWeeklyGoal.Progress = _firstStepWgProgressAux + (double)_stepsViewModel.TodayStepCount;
                        _firstStepWeeklyGoal.CalculatePercentage();
                        
                        Task.Run(async () =>
                        {
                            await WeeklyGoalService.Instance.PutWeeklyGoalByPatientId(_patientId, _firstStepWeeklyGoal);
                        });
                    }
                }
            }
            
        }


        private async void UpdatingDistanceDg()
        {
            //Es decir su id existe en la base de datos
            if (_firstDistanceDg != null)
            {
                //_stepsViewModel.UpdateInfo();
                Debug.Print("Recorridos..." + _stepsViewModel.TodayStepCount * 0.62 + " metros");

                if (_firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount * 0.62)
                    && _stepsViewModel.TodayStepCount != 0)
                {
                    //Si mi contador supero mi objetivo
                    if ((double)(_stepsViewModel.TodayStepCount * 0.62) >= _firstDistanceDg.Quantity)
                    {
                        _firstDistanceDg.Progress = (double)_firstDistanceDg.Quantity;
                        _firstDistanceDg.CalculatePercentage();


                        var resp = await DailyGoalService.Instance.PutDailyGoalDistanceByPatientId(_patientId, _firstDistanceDg);


                        //Al finalizar el objetivo obtenemos el siguiente
                        //Si no hay se inventa localmente uno para rastrear tu avance
                        if(resp != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Actualizamo UI importante
                                //Cuando estamos dentro de un timer
                                UpdateCompletedDailyGoals();
                                GetDailyGoalDistance();

                                //Actualizamos su porcentaje de avance del nuevo objetivo de la cola

                                if (_firstDistanceDg != null)
                                {
                                    _firstDistanceDg.CalculatePercentage();
                                }
                                
                                //Seteamos el nuevo objetivo
                                //Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
                            });
                        }
                        
                    }
                    //Si estoy debajo de mi objetivo
                    else
                    {
                        /*_firstDistanceDg.Progress = Math.Round(_stepsViewModel.TodayStepCount * 0.66, 2);
                        _firstDistanceDg.CalculatePercentage();

                        await DailyGoalService.Instance.PutDailyGoalDistanceByPatientId(_patientId, _firstDistanceDg);*/
                        foreach (var item in _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 3 && e.Progress < e.Quantity).ToList())
                        {
                            item.Progress = Math.Round(_stepsViewModel.TodayStepCount * 0.62, 2);
                            item.CalculatePercentage();
                            await DailyGoalService.Instance.PutDailyGoalDistanceByPatientId(_patientId,
                                item);
                        }
                    }
                }
            }
            else
            {
                Debug.Print("(Registrando para hoy) Contando..." + _stepsViewModel.TodayStepCount * 0.62 + " metros");

            }
            
            //Siempre vamos a estar registrando la distancia
            if (_distanceActivity.Quantity != (double)_stepsViewModel.TodayStepCount*0.62)
            {
                _distanceActivity.Quantity = Math.Round(_stepsViewModel.TodayStepCount * 0.62, 2);
                _distanceActivity.CalculatePercentage();

                await DistanceService.Instance.UpdateDistanceActivityByPatientIdAndId(_patientId,_distanceActivity.Id,_distanceActivity);
            };
            
            
        }

        private async void UpdatingKilocalorieDg()
        {
            //Es decir su id existe en la base de datos
            if (_firstKilocalorieDg != null)
            {
                //_stepsViewModel.UpdateInfo();
                Debug.Print("Kilocalorias quemadas..." + _stepsViewModel.TodayStepCount * 0.018);

                if (_firstKilocalorieDg.Progress != (double)(_stepsViewModel.TodayStepCount * 0.018)
                    && _stepsViewModel.TodayStepCount != 0)
                {
                    //Si mi contador supero mi objetivo
                    if ((double)(_stepsViewModel.TodayStepCount * 0.018) >= _firstKilocalorieDg.Quantity)
                    {
                        _firstKilocalorieDg.Progress = (double)_firstKilocalorieDg.Quantity;
                        _firstKilocalorieDg.CalculatePercentage();


                        var resp = await DailyGoalService.Instance.PutDailyGoalKilocalorieByPatientId(_patientId, _firstKilocalorieDg);


                        //Al finalizar el objetivo obtenemos el siguiente
                        //Si no hay se inventa localmente uno para rastrear tu avance
                        if(resp != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Actualizamo UI importante
                                //Cuando estamos dentro de un timer
                                UpdateCompletedDailyGoals();
                                GetDailyGoalKilocalorie();

                                //Actualizamos su porcentaje de avance del nuevo objetivo de la cola

                                if (_firstKilocalorieDg != null)
                                {
                                    _firstKilocalorieDg.CalculatePercentage();
                                }
                                
                                //Seteamos el nuevo objetivo
                                //Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
                            });
                        }

                        

                    }
                    //Si estoy debajo de mi objetivo
                    else
                    {
                        /*_firstKilocalorieDg.Progress = Math.Round(_stepsViewModel.TodayStepCount * 0.02, 2);
                        _firstKilocalorieDg.CalculatePercentage();*/

                        foreach (var item in _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 2 && e.Progress < e.Quantity).ToList())
                        {
                            item.Progress = Math.Round(_stepsViewModel.TodayStepCount * 0.018, 2);
                            item.CalculatePercentage();
                            await DailyGoalService.Instance.PutDailyGoalKilocalorieByPatientId(_patientId,
                                item);
                        }

                        /*await DailyGoalService.Instance.PutDailyGoalKilocalorieByPatientId(_patientId,
                                _firstKilocalorieDg);*/
                    }
                }
            }
            else
            {
                
                Debug.Print("(Registrando para hoy) Contando..." + _stepsViewModel.TodayStepCount * 0.018 + " Kilocalorias");
            }
            
            
            //Siempre vamos a estar registrando las kilocalorias
            if (_kilocalorieActivity.Quantity != (double)(_stepsViewModel.TodayStepCount * 0.018))
            {
                _kilocalorieActivity.Quantity = Math.Round(_stepsViewModel.TodayStepCount * 0.018, 2);
                _kilocalorieActivity.CalculatePercentage();

                await KilocalorieService.Instance.UpdateKilocalorieActivityByPatientIdAndId(_patientId,_kilocalorieActivity.Id,_kilocalorieActivity);
            };
            
        }

        private void SetupDailyGoal()
        {
            Device.StartTimer(_second, () =>
            {
                Task.Run(async () =>
                {
                    if(Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        _isCheckingSteps = await _stepsViewModel.UpdateInfo();
                        if (!_isCheckingSteps)
                        {
                            await Device.InvokeOnMainThreadAsync(async () =>
                            {
                                _isTimerWorking = false;
                                Windesheart.PairedDevice?.Disconnect();
                                ButtonConnectSmartWatch.IsVisible = true;
                                await DisplayAlert("Mensaje importante",
                                    "Se perdió la conexión con el Xiaomi MiBand, vuelva a conectarlo en ajustes.",
                                    "Ok");
                            });
                        }
                        else
                        {
                            UpdatingStepDg();
                            //UpdatingStepWg();
                            UpdatingDistanceDg();
                            UpdatingKilocalorieDg();
                        }

                        _IsDisplayedNoInternetMsg = false;
                    }
                    else
                    {
                        if (!_IsDisplayedNoInternetMsg)
                        {
                          
                            await Device.InvokeOnMainThreadAsync(async () =>
                            {
                                _IsDisplayedNoInternetMsg = true;
                                await DisplayAlert("Importante",
                                "No cuenta con conexion a internet, restablezca su conexión para continuar", "Aceptar");
                            });
                                
                            
                        }
                        
                    }
                    

                    
                });


               

                //LabelSteps.Text = _stepsViewModel.TodayStepCount.ToString();
                return _isTimerWorking;
            });
        }

        private void UpdateCompletedDailyGoals()
        {
            var completedDgGoals = _dailyGoalsViewModel.DailyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelDgCompleted.Text =
                $"{completedDgGoals.ToString()} / {_dailyGoalsViewModel.DailyGoals.Count.ToString()}";
        }

        private void UpdateCompletedWeeklyGoals()
        {
            var completedWgGoals = _weeklyGoalViewModel.WeeklyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelWgCompleted.Text =
                $"{completedWgGoals.ToString()} / {_weeklyGoalViewModel.WeeklyGoals.Count.ToString()}";
        }


        //Handle Auto-connect to the last connected device with App-properties
        private async void HandleAutoConnect()
        {
            if (Application.Current.Properties.ContainsKey("LastConnectedDevice"))
            {
                var knownGuid = Application.Current.Properties["LastConnectedDevice"].ToString();
                if (!string.IsNullOrEmpty(knownGuid))
                {
                    var knownDevice = await Windesheart.GetKnownDevice(Guid.Parse(knownGuid));

                    try
                    {
                        //No entiendo porque este metodo se pasa por alto sin esperar
                        knownDevice.Connect(CallbackHandler.OnConnect);
                        //

                        await this.DisplayToastAsync("Intentando conectar MiBand 4 registrado ...", 15000);
                        //Esperamos 15 segundos para que conecte
                        //await Task.Delay(10000);

                        if (Windesheart.PairedDevice != null && Windesheart.PairedDevice.IsAuthenticated())
                        {
                            _isTimerWorking = true;
                            SetupDailyGoal();
                            Windesheart.PairedDevice.SetStepGoal(int.Parse(_firstStepDg.Quantity.ToString()));
                            await this.DisplayToastAsync("Conexión exitosa", 3500);
                        }
                        else
                        {
                            await this.DisplayToastAsync("La operación tardó demasiado", 3500);
                        }
                    }
                    catch (Exception _)
                    {
                        await this.DisplayToastAsync("Fallo al conectar", 3500);
                    }
                }
            }
        }

        private async void RegisterSmartwatch_OnClicked(object sender, EventArgs e)
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new DevicePage());
            await Navigation.PushAsync(new DevicePage());
        }

        private async void StepsProgress_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StepsPage(_firstStepDg));
        }

        private async void FrameDailyGoals_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                _dailyGoalsViewModel.UpdateDescriptionDg();
                var dailyGoals = param as ObservableCollection<DailyGoal>;
                if (dailyGoals != null)
                {
                    await Navigation.PushAsync(new CurrentDayReportPage(dailyGoals));
                }
            }
        }

        private async void FrameWeeklyGoals_OnTapped(object sender, EventArgs e)
        {
            var param = ((TappedEventArgs)e).Parameter;
            if (param != null)
            {
                _weeklyGoalViewModel.UpdateDescriptionWg();
                var weeklyGoals = param as ObservableCollection<WeeklyGoal>;
                if (weeklyGoals != null)
                {
                    await Navigation.PushAsync(new CurrentWeekReportPage(weeklyGoals));
                }
            }
        }
    }
}