using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceRecognition.DataLayer.Models;
using FaceRecognition.GUILayer.Models;
using PropertyChanged;
using MirzaCryptoHelpers.Hashings;
using MirzaCryptoHelpers.Common;
using FaceRecognition.DataLayer.Context;

namespace FaceRecognition.GUILayer.Authentication.Login
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel
    {
        public event EventHandler<bool> GoToMainViewRequested;
        public event EventHandler GoToRegisterViewRequested;
        public event EventHandler ExitRequested;

        public LoginUserModel User { get; set; }
        public RelayCommand<LoginUserModel> LoginCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand GoToRegisterCommand { get; set; }

        public LoginViewModel()
        {
            User = new LoginUserModel();
            LoginCommand = new RelayCommand<LoginUserModel>(LoginHandler, CanLoginHandler);
            ExitCommand = new RelayCommand(ExitHandler);
            GoToRegisterCommand = new RelayCommand(GoToRegisterHandler);
        }

        

        #region Handlers
        public bool CanLoginHandler(LoginUserModel user)
        {
            return user != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password);
        }
        public void LoginHandler(LoginUserModel user)
        {
            try
            {
                user.UserName = user.UserName.Trim();
                string hashedPassword = new SHA512Crypto().GetHashBase64String(user.Password);
                using(CoreContext context = new CoreContext())
                {
                    bool exists = context.Users.Any(x => x.UserName.Equals(user.UserName,StringComparison.InvariantCultureIgnoreCase) && x.PasswordHash.Equals(hashedPassword));
                    GoToMainViewRequested?.Invoke(this, exists);
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
