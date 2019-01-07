using FaceRecognition.GUILayer.Authentication.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using FaceRecognition.GUILayer.Authentication.Register;

namespace FaceRecognition.GUILayer.Authentication
{
    [AddINotifyPropertyChangedInterface]
    public class AuthenticationViewModel
    {
        private LoginViewModel _loginViewModel = new LoginViewModel();
        private RegisterViewModel registerViewModel = new RegisterViewModel();
        public object CurrentView { get; set; }
        public AuthenticationViewModel()
        {
            CurrentView = _loginViewModel;
        }
    }
}
