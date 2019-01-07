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
        private LoginUserModel _user = new LoginUserModel();
        private bool _isLoading, _isEnabled;

        public event EventHandler GoToRegisterViewRequested;
        public event EventHandler<LoggedUserModel> GoToMainViewRequested;
        public event EventHandler ExitRequested;
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> ErrorOccured;

       
        
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
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public LoginViewModel()
        {
            IsEnabled = true;
            IsLoading = false;
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
                IsLoading = true;
                IsEnabled = false;
                _user.UserName = _user.UserName.Trim();
                string hashedPassword = new SHA512Crypto().GetHashBase64String(_user.Password);
                LoggedUserModel loggedUser = new LoggedUserModel();
               
                string error = string.Empty;
                await Task.Run(() =>
                {

                    try
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
                    }
                    catch(Exception ex)
                    {
                        error = ex.Message;
                    }
                });
                IsLoading = false;
                IsEnabled = true;
                if (!string.IsNullOrEmpty(error))
                {
                    ErrorOccured?.Invoke(error);
                }
                else
                {
                    GoToMainViewRequested?.Invoke(this, loggedUser);
                }
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
