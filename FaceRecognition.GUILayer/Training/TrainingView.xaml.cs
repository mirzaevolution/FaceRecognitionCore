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

namespace FaceRecognition.GUILayer.Training
{
    /// <summary>
    /// Interaction logic for TrainingView.xaml
    /// </summary>
    public partial class TrainingView : UserControl
    {
        public TrainingView()
        {
            InitializeComponent();
            this.Unloaded += UnloadedHandler;
            InitializeComboBox();
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
    }
}
