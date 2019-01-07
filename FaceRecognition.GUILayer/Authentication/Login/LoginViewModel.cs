using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FaceRecognition.DataLayer.Models;
using FaceRecognition.GUILayer.Models;
using MirzaCryptoHelpers.Hashings;
using MirzaCryptoHelpers.Common;
using FaceRecognition.DataLayer.Context;

namespace FaceRecognition.GUILayer.Authentication.Login
{
    
    public class LoginViewModel:INotifyPropertyChanged
    {
        public event EventHandler GoToRegisterViewRequested;
        public event EventHandler<LoggedUserModel> GoToMainViewRequested;
        public event EventHandler ExitRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        private LoginUserModel _user = new LoginUserModel();

        public LoginUserModel User
        {
            get { return _user; }
            set
            {
                _user = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
            }
        }
        public RelayCommand LoginCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand GoToRegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginHandler, CanLoginHandler);
            ExitCommand = new RelayCommand(ExitHandler);
            GoToRegisterCommand = new RelayCommand(GoToRegisterHandler);
        }

        

        #region Handlers
        public bool CanLoginHandler()
        {
            return _user != null && !string.IsNullOrEmpty(_user.UserName) && !string.IsNullOrEmpty(_user.Password);
        }
        public async void LoginHandler()
        {
            try
            {
                _user.UserName = _user.UserName.Trim();
                string hashedPassword = new SHA512Crypto().GetHashBase64String(_user.Password);
                LoggedUserModel loggedUser = new LoggedUserModel();
                
                await Task.Run(() =>
                {

                    using (CoreContext context = new CoreContext())
                    {
                        User user = context.Users.FirstOrDefault(x => x.UserName.Equals(_user.UserName, StringComparison.InvariantCultureIgnoreCase) && x.PasswordHash.Equals(hashedPassword));
                        if (user != null)
                        {
                            loggedUser.ID = user.ID;
                            loggedUser.UserName = user.UserName;
                            loggedUser.FullName = user.FullName;
                            loggedUser.Email = user.Email;
                            loggedUser.IsAuthenticated = true;
                        }
                        else
                        {
                            loggedUser.IsAuthenticated = true;
                        }
                    }
                });
                 
                GoToMainViewRequested?.Invoke(this, loggedUser);
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }
        private void GoToRegisterHandler()
        {
            GoToRegisterViewRequested?.Invoke(this,EventArgs.Empty);
        }

        private void ExitHandler()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
