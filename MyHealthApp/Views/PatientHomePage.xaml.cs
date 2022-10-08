using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHealthApp.Helpers;
using MyHealthApp.Models;
using MyHealthApp.Services;
using MyHealthApp.Services.MiBand;
using MyHealthApp.ViewModels;
using MyHealthApp.Views.Register;
using ProgressRingControl.Forms.Plugin;
using WindesHeartSDK;
using Xamarin.CommunityToolkit.Extensions;
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
        
        private readonly TimeSpan _second = TimeSpan.FromSeconds(2);
        private readonly StepsViewModel _stepsViewModel;
        private bool _isTimerWorking = false;
        private bool _isCheckingSteps = false;
        private readonly long _patientId;

        private DailyGoal _firstStepDg;
        private DailyGoal _firstDistanceDg;
        private DailyGoal _firstKilocalorieDg;
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
                FlexLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
                
                //Los weeklyGoals
                FlexLayoutWeeklyGoals.BindingContext = _weeklyGoalViewModel;
                
                var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientIdAndDate(_patientId,DateTime.Today);
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
                GetDailyGoalStep();
                GetDailyGoalDistance();
                GetDailyGoalKilocalorie();
                GetGoalsInformation();
            });
            
           
        }
        
        

        private void GetDailyGoalStep()
        {
            try
            {
                _firstStepDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity).ToList().First();
                
                LabelProgressSteps.BindingContext = _firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty,"Progress");

                LabelGoalSteps.BindingContext = _firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default,stringFormat:"/{0}");

                
                ProgressRingSteps.BindingContext = _firstStepDg;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                if (_firstStepDg.Progress == 0.0)
                {
                    _firstStepDg.Progress = _stepsViewModel.TodayStepCount;
                }
                
                //LabelProgressSteps.Text = firstStepDg.Progress.ToString(CultureInfo.CurrentCulture);
                //LabelGoalSteps.Text = "/"+firstStepDg.Quantity.ToString(CultureInfo.CurrentCulture);
                //ProgressRingSteps.Progress = firstStepDg.Percentage;
            }
            catch (InvalidOperationException e)
            {
                
                LabelGoalSteps.TextColor = Color.White;
                
                _firstStepDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 9999.0,
                    Progress = _stepsViewModel.TodayStepCount,
                    ActivityId = 1,
                };
                    
                LabelProgressSteps.BindingContext = _firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty,"Progress");

                LabelGoalSteps.BindingContext = _firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                
                ProgressRingSteps.BindingContext = _firstStepDg;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);
                
                
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
        
        private void GetDailyGoalDistance()
        {
            
            try
            {
                _firstDistanceDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 3 && e.Progress < e.Quantity).ToList().First();
                
                LabelProgressDistance.BindingContext = _firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = _firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default,stringFormat: "{0:#.0} m");

                
                ProgressRingDistance.BindingContext = _firstDistanceDg;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                //LabelProgressDistance.Text = firstDistanceDg.Progress.ToString(CultureInfo.CurrentCulture)+" m";
                //LabelGoalDistance.Text = "/"+firstDistanceDg.Quantity.ToString(CultureInfo.CurrentCulture)+" m";
                //ProgressRingDistance.Progress = firstDistanceDg.Percentage;
                
                if (_firstDistanceDg.Progress == 0.0)
                {
                    _firstDistanceDg.Progress = _stepsViewModel.TodayStepCount*0.762;
                }
                
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingDistance);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingDistance);
                
                LabelGoalDistance.TextColor = Color.White;
                
                _firstDistanceDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 9999.0,
                    Progress = _stepsViewModel.TodayStepCount*0.762, //Converting Steps to meters
                    ActivityId = 3,
                };
                
                LabelProgressDistance.BindingContext = _firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = _firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                
                ProgressRingDistance.BindingContext = _firstDistanceDg;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                
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

        private void GetDailyGoalKilocalorie()
        {
            try
            {
                _firstKilocalorieDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 2 && e.Progress < e.Quantity).ToList().First();
                
                LabelProgresskilocalorie.BindingContext = _firstKilocalorieDg;
                LabelProgresskilocalorie.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.00} kcal");

                LabelGoalkilocalorie.BindingContext = _firstKilocalorieDg;
                LabelGoalkilocalorie.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default,stringFormat: "{0:#.00} kcal");

                
                ProgressRingKilocalorie.BindingContext = _firstKilocalorieDg;
                ProgressRingKilocalorie.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                //LabelProgressDistance.Text = firstDistanceDg.Progress.ToString(CultureInfo.CurrentCulture)+" m";
                //LabelGoalDistance.Text = "/"+firstDistanceDg.Quantity.ToString(CultureInfo.CurrentCulture)+" m";
                //ProgressRingDistance.Progress = firstDistanceDg.Percentage;
                
                if (_firstKilocalorieDg.Progress == 0.0)
                {
                    _firstKilocalorieDg.Progress = _stepsViewModel.TodayStepCount* 0.04/1000;
                }
                
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingDistance);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingDistance);
                
                LabelGoalkilocalorie.TextColor = Color.White;
                
                _firstKilocalorieDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 9999.0,
                    Progress = _stepsViewModel.TodayStepCount*0.04/1000, //Converting Steps to calories
                    ActivityId = 2,
                };
                
                LabelProgresskilocalorie.BindingContext = _firstKilocalorieDg;
                LabelProgresskilocalorie.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.00} kcal");

                LabelGoalkilocalorie.BindingContext = _firstKilocalorieDg;
                LabelGoalkilocalorie.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                
                ProgressRingKilocalorie.BindingContext = _firstKilocalorieDg;
                ProgressRingKilocalorie.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);
                
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
                _isTimerWorking = true;
                SetupDailyGoal();
                Windesheart.PairedDevice?.SetStepGoal(int.Parse(_firstStepDg.Quantity.ToString()));
            }
            else
            {
                //HandleAutoConnect();
            }
        }

        protected override void OnDisappearing()
        {
            _isTimerWorking = false;
        }

        private void UpdatingStepDg()
        {
            //Es decir su id existe en la base de datos
                if (_firstStepDg.Id != -1)
                {
                    //_stepsViewModel.UpdateInfo();
                    Debug.Print("Contando..." +_stepsViewModel.TodayStepCount.ToString()+" pasos");
                  
                    if (_firstStepDg.Progress != (double)_stepsViewModel.TodayStepCount
                        && _stepsViewModel.TodayStepCount != 0)
                    {
                        
                        //Si mi contador supero mi objetivo
                        if ((double)_stepsViewModel.TodayStepCount > _firstStepDg.Quantity)
                        {
                           
                            _firstStepDg.Progress = (double)_firstStepDg.Quantity;
                            _firstStepDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstStepDg);
                                
                                
                                //Al finalizar el objetivo obtenemos el siguiente
                                //Si no hay se inventa localmente uno para rastrear tu avance
                                
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //Actualizamo UI importante
                                    //Cuando estamos dentro de un timer
                                    UpdateCompletedDailyGoals();
                                    GetDailyGoalStep();
                                    
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola
                                    
                                    _firstStepDg.CalculatePercentage();
                                    //Seteamos el nuevo objetivo
                                    Windesheart.PairedDevice.SetStepGoal(int.Parse(_firstStepDg.Quantity.ToString()));
                                });

                                
                                //Si no hay entonces cerramos el timer
                                //_isTimerWorking = (firstStepDg != null);
                            });
                            
                        }
                        //Si estoy debajo de mi objetivo
                        else
                        {
                            _firstStepDg.Progress = (double)_stepsViewModel.TodayStepCount; 
                            _firstStepDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstStepDg);
                            });
                        }
                    
                    }
                }
                else
                {
                    
                    Debug.Print("(local) Contando..." +_stepsViewModel.TodayStepCount.ToString()+" pasos");

                    if (_firstStepDg.Progress != (double)_stepsViewModel.TodayStepCount)
                    {
                        _firstStepDg.Progress = (double)_stepsViewModel.TodayStepCount;
                        _firstStepDg.CalculatePercentage();
                    };
                    
                    
                    //_isTimerWorking = false;
                }
        }


        private void UpdatingDistanceDg()
        {
            //Es decir su id existe en la base de datos
                if (_firstDistanceDg.Id != -1)
                {
                    //_stepsViewModel.UpdateInfo();
                    Debug.Print("Recorridos..." +_stepsViewModel.TodayStepCount*0.762 +" metros");
                  
                    if (_firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.762)
                        && _stepsViewModel.TodayStepCount != 0)
                    {
                        
                        //Si mi contador supero mi objetivo
                        if ((double)(_stepsViewModel.TodayStepCount*0.762) > _firstDistanceDg.Quantity)
                        {
                           
                            _firstDistanceDg.Progress = (double)_firstDistanceDg.Quantity;
                            _firstDistanceDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstDistanceDg);
                                
                                
                                //Al finalizar el objetivo obtenemos el siguiente
                                //Si no hay se inventa localmente uno para rastrear tu avance
                                
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //Actualizamo UI importante
                                    //Cuando estamos dentro de un timer
                                    UpdateCompletedDailyGoals();
                                    GetDailyGoalDistance();
                                    
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola
                                    
                                    _firstDistanceDg.CalculatePercentage();
                                    //Seteamos el nuevo objetivo
                                    //Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
                                });

                                
                                //Si no hay entonces cerramos el timer
                                //_isTimerWorking = (firstStepDg != null);
                            });
                            
                        }
                        //Si estoy debajo de mi objetivo
                        else
                        {
                            _firstDistanceDg.Progress = (double)(_stepsViewModel.TodayStepCount*0.762); 
                            _firstDistanceDg.CalculatePercentage();
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstDistanceDg);
                            });
                        }
                    
                    }
                }
                else
                {
                    
                    Debug.Print("(local) Recorridos..." +_stepsViewModel.TodayStepCount*0.762 +" metros");

                    if (_firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.762))
                    {
                        _firstDistanceDg.Progress = (_stepsViewModel.TodayStepCount*0.762);
                        _firstDistanceDg.CalculatePercentage();
                    };
                    
                    
                    //_isTimerWorking = false;
                }
        }
        
        private void UpdatingKilocalorieDg()
        {
            //Es decir su id existe en la base de datos
                if (_firstKilocalorieDg.Id != -1)
                {
                    //_stepsViewModel.UpdateInfo();
                    Debug.Print("Kilocalorias quemadas..." +_stepsViewModel.TodayStepCount*0.04/1000);
                  
                    if (_firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.04/1000)
                        && _stepsViewModel.TodayStepCount != 0)
                    {
                        
                        //Si mi contador supero mi objetivo
                        if ((double)(_stepsViewModel.TodayStepCount*0.04/1000) > _firstDistanceDg.Quantity)
                        {
                           
                            _firstKilocalorieDg.Progress = (double)_firstKilocalorieDg.Quantity;
                            _firstKilocalorieDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstKilocalorieDg);
                                
                                
                                //Al finalizar el objetivo obtenemos el siguiente
                                //Si no hay se inventa localmente uno para rastrear tu avance
                                
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //Actualizamo UI importante
                                    //Cuando estamos dentro de un timer
                                    UpdateCompletedDailyGoals();
                                    GetDailyGoalKilocalorie();
                                    
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola
                                    
                                    _firstKilocalorieDg.CalculatePercentage();
                                    //Seteamos el nuevo objetivo
                                    //Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
                                });

                                
                                //Si no hay entonces cerramos el timer
                                //_isTimerWorking = (firstStepDg != null);
                            });
                            
                        }
                        //Si estoy debajo de mi objetivo
                        else
                        {
                            _firstKilocalorieDg.Progress = (double)(_stepsViewModel.TodayStepCount*0.04/1000); 
                            _firstKilocalorieDg.CalculatePercentage();
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, _firstKilocalorieDg);
                            });
                        }
                    
                    }
                }
                else
                {
                    
                    Debug.Print("Kilocalorias quemadas..." +_stepsViewModel.TodayStepCount*0.04/1000);

                    if (_firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.04/1000))
                    {
                        _firstKilocalorieDg.Progress = (_stepsViewModel.TodayStepCount*0.04/1000);
                        _firstKilocalorieDg.CalculatePercentage();
                    };
                    
                    
                    //_isTimerWorking = false;
                }
        }

        private void SetupDailyGoal()
        {
            Device.StartTimer(_second, () =>
            {

                Task.Run(async () =>
                {
                    _isCheckingSteps = await _stepsViewModel.UpdateInfo();
                    if (!_isCheckingSteps)
                    {
                        await Device.InvokeOnMainThreadAsync( async () =>
                        {
                            _isTimerWorking = false;
                            Windesheart.PairedDevice?.Disconnect();
                            await DisplayAlert("Mensaje importante", 
                                "Se perdió la conexión con el Xiaomi MiBand, vuelva a conectarlo en ajustes.",
                                "Ok");
                        });
                    }
                });
                
                
                UpdatingStepDg();
                UpdatingDistanceDg();
                UpdatingKilocalorieDg();

                //LabelSteps.Text = _stepsViewModel.TodayStepCount.ToString();
                return _isTimerWorking;
            });
        }

        private void UpdateCompletedDailyGoals()
        {
            var completedDgGoals = _dailyGoalsViewModel.DailyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelDgCompleted.Text = $"{completedDgGoals.ToString()} / {_dailyGoalsViewModel.DailyGoals.Count.ToString()}";
        }
        
        private void UpdateCompletedWeeklyGoals()
        {
            var completedWgGoals = _weeklyGoalViewModel.WeeklyGoals.Count(goal => goal.Progress == goal.Quantity);
            LabelWgCompleted.Text = $"{completedWgGoals.ToString()} / {_weeklyGoalViewModel.WeeklyGoals.Count.ToString()}";
        }


        //Handle Auto-connect to the last connected device with App-properties
        private async void HandleAutoConnect()
        {
            if (Application.Current.Properties.ContainsKey("LastConnectedDevice"))
            {
                var knownGuid =  Application.Current.Properties["LastConnectedDevice"].ToString();
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
        
    }
}