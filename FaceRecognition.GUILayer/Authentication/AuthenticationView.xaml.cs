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
using System.Windows.Shapes;
namespace FaceRecognition.GUILayer.Authentication
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : MetroWindow
    {
        public AuthenticationView()
        {
            InitializeComponent();
        }
        

        private async void AuthenticationFailedHandler(object sender, EventArgs e)
        {
            await this.ShowMessageAsync("Loggin Failed", "Your username or password is invalid.");
        }

        private void ExitAppHandler(object sender, LoggedUserModel e)
        {
            if (e!=null)
            {
                Global.LoggedUser = e;
                new MainWindow().Show();
                this.Close();
            }
        }
    }
}
