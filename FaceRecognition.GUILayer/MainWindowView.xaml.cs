using FaceRecognition.GUILayer.ChangeProfile;
using FaceRecognition.GUILayer.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

namespace FaceRecognition.GUILayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void ResizeScreenHandler(object sender, Tuple<double,double> e)
        {
            
            this.Width = e.Item1;
            this.Height = e.Item2;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ErrorHandler(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InformationHandler(string message)
        {
            TextStatus.Text = message;
        }

        private void SignOutHandler(object sender, RoutedEventArgs e)
        {

            Global.LoggedUser = null;
            new Authentication.AuthenticationView().Show();
            this.Close();
        }

        private void UpdateProfileHandler(object sender, RoutedEventArgs e)
        {
            ChangeProfileView view = new ChangeProfileView();
            view.ShowDialog();
        }
    }
}
