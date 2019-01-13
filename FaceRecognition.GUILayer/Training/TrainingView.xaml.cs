using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using WebEye.Controls.Wpf;
using FaceRecognition.GUILayer.Helpers;
using MahApps.Metro.Controls;
using System.IO;
using System.Drawing.Imaging;
using System.Timers;
using System.Threading;

namespace FaceRecognition.GUILayer.Training
{
    /// <summary>
    /// Interaction logic for TrainingView.xaml
    /// </summary>
    public partial class TrainingView : UserControl
    {
        private System.Timers.Timer _timer;
        private static object _syncLock = new object();
        private int _counter;
        private static int COUNTER_MAX = 5;
        public TrainingView()
        {
            InitializeComponent();
            this.Unloaded += UnloadedHandler;
            _timer = new System.Timers.Timer
            {
                Interval = 1500
            };
            _timer.Elapsed += TimerElapsedHandler;
            InitializeComboBox();
            
        }

        private async void TimerElapsedHandler(object sender, ElapsedEventArgs e)
        {
            try
            {

                Dispatcher.Invoke(() => TextCounter.Text = (_counter).ToString());


                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        var bitmap = WebCamCoreControl.GetCurrentImage();

                        if (DataContext is TrainingViewModel repo)
                        {

                            Random random = new Random();
                            var newData = new Models.RepositoryModel
                            {
                                UserID = Global.LoggedUser.ID,
                                Image = BitmapConversion.BitmapToBitmapSource(bitmap),
                                Description = $"Capture #{random.Next(1000000, 10000001)}"
                            };
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ms.Position = 0;
                                bitmap.Save(ms, ImageFormat.Png);
                                ms.Position = 0;
                                newData.SampleImage = ms.ToArray();
                            }
                            repo.MultiCaptureStorages.Add(newData);
                        }
                    },System.Windows.Threading.DispatcherPriority.Background);
                    lock (_syncLock)
                    {
                        if (_counter < COUNTER_MAX)
                        {
                            Interlocked.Increment(ref _counter);
                        }
                        else
                        {
                            _timer.Stop();
                            _counter = 0;
                            Dispatcher.Invoke(() =>
                            {
                                if (DataContext is TrainingViewModel repo)
                                {
                                    repo.AddMultiples();
                                }
                                ListBoxImages.IsEnabled = true;
                                WrapPanelButtons.IsEnabled = true;
                                ButtonCapture.IsEnabled = true;
                                ButtonMultiCapture.IsEnabled = true;

                                ImagePreview.Visibility = Visibility.Visible;
                                TextCounter.Visibility = Visibility.Collapsed;
                            }, System.Windows.Threading.DispatcherPriority.Background);
                        }
                    }
                });
              
            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.Message });
            }
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            if (WebCamCoreControl.IsCapturing)
            {
                WebCamCoreControl.StopCapture();
            }
            
        }

        private async void InitializeComboBox()
        {
           
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        ComboBoxDevices.ItemsSource = WebCamCoreControl.GetVideoCaptureDevices();

                        if (ComboBoxDevices.Items.Count > 0)
                        {
                            ComboBoxDevices.SelectedItem = ComboBoxDevices.Items[0];
                        }
                    }, System.Windows.Threading.DispatcherPriority.Background);
                });
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private async void ButtonCaptureHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        ListBoxImages.SelectedItem = null;
                        var bitmap = WebCamCoreControl.GetCurrentImage();
                        var repo = DataContext as TrainingViewModel;

                        if (repo != null)
                        {
                            repo.IsCaptured = true;
                            repo.IsSaved = false;
                            repo.Repository = null;
                            repo.Repository = new Models.RepositoryModel
                            {
                                UserID = Global.LoggedUser.ID,
                                Image = BitmapConversion.BitmapToBitmapSource(bitmap)
                            };
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ms.Position = 0;
                                bitmap.Save(ms, ImageFormat.Png);
                                ms.Position = 0;
                                repo.Repository.SampleImage = ms.ToArray();
                            }

                        }
                    });
                });
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.Message });
            }
        }

  

        private async void ButtonInitializeHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxDevices.Items.Count > 0)
                {
                    await Task.Run(() =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (WebCamCoreControl.IsCapturing)
                            {
                                WebCamCoreControl.StopCapture();
                            }
                            var cameraId = (WebCameraId)ComboBoxDevices.SelectedItem;
                            WebCamCoreControl.StartCapture(cameraId);
                            ButtonCapture.IsEnabled = true;
                            ButtonInitialize.IsEnabled = false;
                        }, System.Windows.Threading.DispatcherPriority.Background);
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private void ListBoxSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TrainingViewModel repo)
            {
                repo.IsSaved = true;
                repo.IsCaptured = false;
            }
        }

        private async void ComboBoxDevicesSelectionChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (WebCamCoreControl.IsCapturing)
                        {
                            WebCamCoreControl.StopCapture();
                        }
                        var cameraId = (WebCameraId)ComboBoxDevices.SelectedItem;
                        WebCamCoreControl.StartCapture(cameraId);
                        ButtonCapture.IsEnabled = true;
                        ButtonInitialize.IsEnabled = false;
                    }, System.Windows.Threading.DispatcherPriority.Loaded);
                });
                Cursor = null;
                //ButtonInitialize.IsEnabled = true;
                //ButtonCapture.IsEnabled = false;
                //WebCamCoreControl.StopCapture();
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private void ButtonMultiCaptureHandler(object sender, RoutedEventArgs e)
        {
            if (DataContext is TrainingViewModel repo)
            {
                repo.Repository = null;
                ListBoxImages.SelectedItem = null;
                ListBoxImages.IsEnabled = false;
                WrapPanelButtons.IsEnabled = false;
                ButtonCapture.IsEnabled = false;
                ButtonMultiCapture.IsEnabled = false;
                repo.MultiCaptureStorages.Clear();
                ImagePreview.Visibility = Visibility.Collapsed;
                TextCounter.Visibility = Visibility.Visible;
                _counter = 1;
                TextCounter.Text = _counter.ToString();
                _timer.Start();
            }
        }
    }
}
