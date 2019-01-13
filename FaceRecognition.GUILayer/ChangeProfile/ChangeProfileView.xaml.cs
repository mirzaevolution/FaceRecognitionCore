using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace FaceRecognition.GUILayer.ChangeProfile
{
    /// <summary>
    /// Interaction logic for ChangeProfileView.xaml
    /// </summary>
    public partial class ChangeProfileView : MetroWindow
    {
        public ChangeProfileView()
        {
            InitializeComponent();
        }

        private void CurrentPasswordBoxFieldChanged(object sender, RoutedEventArgs e)
        {
            ((ChangeProfileViewModel)DataContext).CurrentUser.CurrentPassword = CurrentPasswordField.Password;
        }

        private void NewPasswordBoxFieldChanged(object sender, RoutedEventArgs e)
        {
            ((ChangeProfileViewModel)DataContext).CurrentUser.Password = NewPasswordField.Password;

        }

        private async void ChangeProfileViewModel_Information(string message)
        {
            await this.ShowMessageAsync("Information", message);
        }

        private async void ChangeProfileViewModel_ErrorOccured(string error)
        {
            await this.ShowMessageAsync("Error", error);
        }

        private void ChangeProfileViewModel_ExitRequested(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
