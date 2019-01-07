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
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {
           
            try
            {
                ComboBoxDevices.ItemsSource = WebCamCoreControl.GetVideoCaptureDevices();

                if (ComboBoxDevices.Items.Count > 0)
                {
                    ComboBoxDevices.SelectedItem = ComboBoxDevices.Items[0];
                    
                }
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private void ButtonCaptureHandler(object sender, RoutedEventArgs e)
        {

            try
            {
                var bitmap = WebCamCoreControl.GetCurrentImage();
                var repo = DataContext as TrainingViewModel;
                if (repo != null)
                {
                    if (repo.Repository == null)
                    {
                        repo.Repository = new Models.RepositoryModel();
                    }
                    repo.Repository.UserID = Global.LoggedUser.ID;
                    repo.Repository.Image = BitmapConversion.BitmapToBitmapSource(bitmap);
                    using(MemoryStream ms = new MemoryStream())
                    {
                        ms.Position = 0;
                        bitmap.Save(ms, ImageFormat.Png);
                        ms.Position = 0;
                        repo.Repository.SampleImage = ms.ToArray();
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.Message });
            }
            
        }

  

        private void ButtonInitializeHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBoxDevices.Items.Count > 0)
                {
                    if (WebCamCoreControl.IsCapturing)
                    {
                        WebCamCoreControl.StopCapture();
                    }
                    var cameraId = (WebCameraId)ComboBoxDevices.SelectedItem;
                    WebCamCoreControl.StartCapture(cameraId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }
    }
}
