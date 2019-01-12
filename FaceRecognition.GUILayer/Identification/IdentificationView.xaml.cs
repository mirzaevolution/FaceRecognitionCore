﻿using System;
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
using FaceRecognition.GUILayer.Models;

namespace FaceRecognition.GUILayer.Identification
{
    /// <summary>
    /// Interaction logic for IdentificationView.xaml
    /// </summary>
    public partial class IdentificationView : UserControl
    {
        public IdentificationView()
        {
            InitializeComponent();
            InitializeComboBox();
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
            catch (Exception ex)
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
                        var bitmap = WebCamCoreControl.GetCurrentImage();

                        if (DataContext is IdentificationViewModel repo)
                        {

                            repo.PreviewImage = new PreviewImageModel
                            {
                                Image = BitmapConversion.BitmapToBitmapSource(bitmap)
                            };
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ms.Position = 0;
                                bitmap.Save(ms, ImageFormat.Png);
                                ms.Position = 0;
                                repo.PreviewImage.ImageBytes = ms.ToArray();
                            }
                            repo.PreviewImageCloned = new PreviewImageModel()
                            {
                                Image = repo.PreviewImage.Image.Clone(),
                                ImageBytes = repo.PreviewImage.ImageBytes.Clone() as byte[]
                            };
                        }
                    }, System.Windows.Threading.DispatcherPriority.Background);
                });
                if(DataContext is IdentificationViewModel data)
                {
                    data.IsCaptured = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.Message });
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
                    }, System.Windows.Threading.DispatcherPriority.Background);
                });
                Cursor = null;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private void UnloadedHandler(object sender, RoutedEventArgs e)
        {
            if (WebCamCoreControl.IsCapturing)
            {
                WebCamCoreControl.StopCapture();
            }
        }
    }

}
