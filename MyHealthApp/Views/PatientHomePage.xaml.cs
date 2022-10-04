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
        private readonly string _propertyKey = "LastConnectedDevice";
        private PatientDailyGoalsViewModel _dailyGoalsViewModel;
        private PatientWeeklyGoalViewModel _weeklyGoalViewModel;
        private readonly ValueToDoubleConverter _valueToDoubleConverter = new ValueToDoubleConverter();
        
        private readonly TimeSpan _second = TimeSpan.FromSeconds(2);
        private StepsViewModel _stepsViewModel;
        private bool _isTimerWorking = false;
        private bool _isCheckingSteps = false;
        private long _patientId;

        DailyGoal firstStepDg;
        private DailyGoal firstDistanceDg;
        public PatientHomePage()
        {
            if (SuccessfulRegisterPage.DailyGoals != null && SuccessfulRegisterPage.DailyGoals != null)
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(SuccessfulRegisterPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(SuccessfulRegisterPage.WeeklyGoals);
            }
            else
            {
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(LoginPage.DailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(LoginPage.WeeklyGoals);
            }
            
            _stepsViewModel = new StepsViewModel();
            _patientId = long.Parse(Application.Current.Properties["PatientId"].ToString());
            
            GetDailyGoalsAndWeeklyGoals();
            InitializeComponent();
            
            
        }



        private async void GetDailyGoalsAndWeeklyGoals()
        {
            
            /*await Device.InvokeOnMainThreadAsync(async () =>
            {
                var dailyGoals = await DailyGoalService.Instance.GetDailyGoalsByPatientId(_patientId);
                var weeklyGoals = await WeeklyGoalService.Instance.GetWeeklyGoalsByPatientId(_patientId);
                _dailyGoalsViewModel = new PatientDailyGoalsViewModel(dailyGoals);
                _weeklyGoalViewModel = new PatientWeeklyGoalViewModel(weeklyGoals);
                //------------------------------------------------
                
            });*/
            Device.BeginInvokeOnMainThread(() =>
            {
                GetDailyGoalStep();
                GetDailyGoalDistance();
                GetGoalsInformation();
            });
            
           
        }
        
        

        private void GetDailyGoalStep()
        {
            try
            {
                firstStepDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 1 && e.Progress < e.Quantity).ToList().First();
                
                LabelProgressSteps.BindingContext = firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty,"Progress");

                LabelGoalSteps.BindingContext = firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default,stringFormat:"/{0}");

                
                ProgressRingSteps.BindingContext = firstStepDg;
                ProgressRingSteps.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                if (firstStepDg.Progress == 0.0)
                {
                    firstStepDg.Progress = _stepsViewModel.TodayStepCount;
                }
                
                //LabelProgressSteps.Text = firstStepDg.Progress.ToString(CultureInfo.CurrentCulture);
                //LabelGoalSteps.Text = "/"+firstStepDg.Quantity.ToString(CultureInfo.CurrentCulture);
                //ProgressRingSteps.Progress = firstStepDg.Percentage;
            }
            catch (InvalidOperationException e)
            {
                
                LabelGoalSteps.TextColor = Color.White;
                
                firstStepDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 9999.0,
                    Progress = _stepsViewModel.TodayStepCount,
                    ActivityId = 1,
                };
                    
                LabelProgressSteps.BindingContext = firstStepDg;
                LabelProgressSteps.SetBinding(Label.TextProperty,"Progress");

                LabelGoalSteps.BindingContext = firstStepDg;
                LabelGoalSteps.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                
                ProgressRingSteps.BindingContext = firstStepDg;
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
                firstDistanceDg = _dailyGoalsViewModel.DailyGoals.Where(e => e.ActivityId == 3 && e.Progress < e.Quantity).ToList().First();
                
                LabelProgressDistance.BindingContext = firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default,stringFormat: "{0:#.0} m");

                
                ProgressRingDistance.BindingContext = firstDistanceDg;
                ProgressRingDistance.SetBinding(ProgressRing.ProgressProperty,"Percentage",BindingMode.Default,_valueToDoubleConverter);

                //LabelProgressDistance.Text = firstDistanceDg.Progress.ToString(CultureInfo.CurrentCulture)+" m";
                //LabelGoalDistance.Text = "/"+firstDistanceDg.Quantity.ToString(CultureInfo.CurrentCulture)+" m";
                //ProgressRingDistance.Progress = firstDistanceDg.Percentage;
                
                if (firstDistanceDg.Progress == 0.0)
                {
                    firstDistanceDg.Progress = _stepsViewModel.TodayStepCount*0.762;
                }
                
            }
            catch (InvalidOperationException e1)
            {
                //FlexLayoutRingsToday.Children.Remove(ProgressRingDistance);
                //FlexLayoutRingsInfoToday.Children.Remove(StackLayoutInfoRingDistance);
                
                LabelGoalDistance.TextColor = Color.White;
                
                firstDistanceDg = new DailyGoal()
                {
                    Id = -1,
                    Percentage = 0,
                    Quantity = 9999.0,
                    Progress = _stepsViewModel.TodayStepCount*0.762, //Converting Steps to meters
                    ActivityId = 3,
                };
                
                LabelProgressDistance.BindingContext = firstDistanceDg;
                LabelProgressDistance.SetBinding(Label.TextProperty,"Progress",stringFormat: "{0:#.0} m");

                LabelGoalDistance.BindingContext = firstDistanceDg;
                LabelGoalDistance.SetBinding(Label.TextProperty,"Quantity",BindingMode.Default);

                
                ProgressRingDistance.BindingContext = firstDistanceDg;
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

        private void GetGoalsInformation()
        {
            
            UpdateCompletedDailyGoals(); 
            UpdateCompletedWeeklyGoals(); 
            //Los dailyGoals
            FlexLayoutDailyGoals.BindingContext = _dailyGoalsViewModel;
            BindableLayout.SetItemsSource(FlexLayoutDailyGoals,_dailyGoalsViewModel.DailyGoals);
            //Debug.WriteLine("Tengo hijos: " + FlexLayoutDailyGoals.Children.Count);
            //FlexLayoutDailyGoals.SetBinding(BindableLayout.ItemsSourceProperty,"DailyGoals");

            //Los weeklyGoals
            FlexLayoutWeeklyGoals.BindingContext = _weeklyGoalViewModel;
            BindableLayout.SetItemsSource(FlexLayoutWeeklyGoals,_weeklyGoalViewModel.WeeklyGoals);
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
                Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
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
                if (firstStepDg.Id != -1)
                {
                    //_stepsViewModel.UpdateInfo();
                    Debug.Print("Contando..." +_stepsViewModel.TodayStepCount.ToString()+" pasos");
                  
                    if (firstStepDg.Progress != (double)_stepsViewModel.TodayStepCount
                        && _stepsViewModel.TodayStepCount != 0)
                    {
                        
                        //Si mi contador supero mi objetivo
                        if ((double)_stepsViewModel.TodayStepCount > firstStepDg.Quantity)
                        {
                           
                            firstStepDg.Progress = (double)firstStepDg.Quantity;
                            firstStepDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, firstStepDg);
                                
                                
                                //Al finalizar el objetivo obtenemos el siguiente
                                //Si no hay se inventa localmente uno para rastrear tu avance
                                
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //Actualizamo UI importante
                                    //Cuando estamos dentro de un timer
                                    UpdateCompletedDailyGoals();
                                    GetDailyGoalStep();
                                    
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola
                                    
                                    firstStepDg.CalculatePercentage();
                                    //Seteamos el nuevo objetivo
                                    Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
                                });

                                
                                //Si no hay entonces cerramos el timer
                                //_isTimerWorking = (firstStepDg != null);
                            });
                            
                        }
                        //Si estoy debajo de mi objetivo
                        else
                        {
                            firstStepDg.Progress = (double)_stepsViewModel.TodayStepCount; 
                            firstStepDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, firstStepDg);
                            });
                        }
                    
                    }
                }
                else
                {
                    
                    Debug.Print("(local) Contando..." +_stepsViewModel.TodayStepCount.ToString()+" pasos");

                    if (firstStepDg.Progress != (double)_stepsViewModel.TodayStepCount)
                    {
                        firstStepDg.Progress = (double)_stepsViewModel.TodayStepCount;
                        firstStepDg.CalculatePercentage();
                    };
                    
                    
                    //_isTimerWorking = false;
                }
        }


        private void UpdatingDistanceDg()
        {
            //Es decir su id existe en la base de datos
                if (firstDistanceDg.Id != -1)
                {
                    //_stepsViewModel.UpdateInfo();
                    Debug.Print("Recorridos..." +_stepsViewModel.TodayStepCount*0.762 +" metros");
                  
                    if (firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.762)
                        && _stepsViewModel.TodayStepCount != 0)
                    {
                        
                        //Si mi contador supero mi objetivo
                        if ((double)(_stepsViewModel.TodayStepCount*0.762) > firstDistanceDg.Quantity)
                        {
                           
                            firstDistanceDg.Progress = (double)firstDistanceDg.Quantity;
                            firstDistanceDg.CalculatePercentage();
                            
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, firstDistanceDg);
                                
                                
                                //Al finalizar el objetivo obtenemos el siguiente
                                //Si no hay se inventa localmente uno para rastrear tu avance
                                
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    //Actualizamo UI importante
                                    //Cuando estamos dentro de un timer
                                    UpdateCompletedDailyGoals();
                                    GetDailyGoalDistance();
                                    
                                    //Actualizamos su porcentaje de avance del nuevo objetivo de la cola
                                    
                                    firstDistanceDg.CalculatePercentage();
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
                            firstDistanceDg.Progress = (double)(_stepsViewModel.TodayStepCount*0.762); 
                            firstDistanceDg.CalculatePercentage();
                            
                            Task.Run(async () =>
                            {
                                await DailyGoalService.Instance.PutDailyGoalByPatientId(_patientId, firstDistanceDg);
                            });
                        }
                    
                    }
                }
                else
                {
                    
                    Debug.Print("(local) Recorridos..." +_stepsViewModel.TodayStepCount*0.762 +" metros");

                    if (firstDistanceDg.Progress != (double)(_stepsViewModel.TodayStepCount*0.762))
                    {
                        firstDistanceDg.Progress = (_stepsViewModel.TodayStepCount*0.762);
                        firstDistanceDg.CalculatePercentage();
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
                            Windesheart.PairedDevice.SetStepGoal(int.Parse(firstStepDg.Quantity.ToString()));
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
            await Navigation.PushAsync(new StepsPage(firstStepDg));
        }
        
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}