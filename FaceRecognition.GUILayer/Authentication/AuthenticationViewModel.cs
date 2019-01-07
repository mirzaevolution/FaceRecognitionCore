using FaceRecognition.GUILayer.Authentication.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FaceRecognition.GUILayer.Authentication.Register;
using FaceRecognition.GUILayer.Models;

namespace FaceRecognition.GUILayer.Authentication
{
    public class AuthenticationViewModel:INotifyPropertyChanged
    {
        private object _currentView;
        private LoginViewModel _loginViewModel = new LoginViewModel();
        private RegisterViewModel _registerViewModel = new RegisterViewModel();

        public event EventHandler<LoggedUserModel> ExitAppRequested;
        public event EventHandler AuthenticationFailed;
        public event PropertyChangedEventHandler PropertyChanged;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if(_currentView!=value)
                {
                    _currentView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
                }
            }

        }
        public AuthenticationViewModel()
        {
            _loginViewModel.GoToMainViewRequested += GoToMainViewHandler;
            _loginViewModel.GoToRegisterViewRequested += GoToRegisterHandler;
            _loginViewModel.ExitRequested += ExitHandler;
            _registerViewModel.GoToMainViewRequested += GoToMainViewHandler;
            _registerViewModel.GoToLoginViewRequested += GoToLoginHandler;
            _registerViewModel.ExitRequested += ExitHandler;
            CurrentView = _loginViewModel;
        }

        private void ExitHandler(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void GoToRegisterHandler(object sender, EventArgs e)
        {
            CurrentView = _registerViewModel;
        }
        private void GoToLoginHandler(object sender, EventArgs e)
        {
            CurrentView = _loginViewModel;
        }
        private void GoToMainViewHandler(object sender, LoggedUserModel e)
        {
            if (e.IsAuthenticated)
            {
                
                ExitAppRequested?.Invoke(this, e);
            }
            else
            {
                AuthenticationFailed?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
