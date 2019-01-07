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
namespace FaceRecognition.GUILayer.Authentication.Register
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private RegisterModel _userRegisterModel = new RegisterModel();

        public RegisterModel UserRegisterModel
        {
            get { return _userRegisterModel; }
            set
            {
                if(_userRegisterModel!=value)
                {
                    _userRegisterModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserRegisterModel)));
                }
            }
        }

        public event EventHandler GoToLoginViewRequested;
        public event EventHandler<LoggedUserModel> GoToMainViewRequested;
        public event EventHandler ExitRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand RegisterCommand { get; set; }       
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand GoToLoginCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(RegisterHandler, CanRegisterHandler);
            ExitCommand = new RelayCommand(ExitHandller);
            GoToLoginCommand = new RelayCommand(GoToLoginHandler);
        }

        private void GoToLoginHandler()
        {
            GoToLoginViewRequested?.Invoke(this, EventArgs.Empty);

        }

        private void ExitHandller()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        private bool CanRegisterHandler()
        {
            return  !string.IsNullOrEmpty(UserRegisterModel.UserName) &&
                    !string.IsNullOrEmpty(UserRegisterModel.FullName) &&
                    !string.IsNullOrEmpty(UserRegisterModel.Email) &&
                    !string.IsNullOrEmpty(UserRegisterModel.Password);
        }

        private async void RegisterHandler()
        {
            bool success = true;
            try
            {
                LoggedUserModel loggedUser = new LoggedUserModel();
                await Task.Run(() =>
                {
                    try
                    {
                        using(CoreContext context = new CoreContext())
                        {
                            User user = new User
                            {
                                UserName = UserRegisterModel.UserName,
                                Email = UserRegisterModel.Email,
                                FullName = UserRegisterModel.FullName
                            };
                            user.PasswordHash = new SHA512Crypto().GetHashBase64String(UserRegisterModel.Password);
                            context.Users.Add(user);
                            int i =context.SaveChanges();
                            success = i > 0;
                            if (success)
                            {

                                loggedUser.ID = user.ID;
                                loggedUser.UserName = user.UserName;
                                loggedUser.FullName = user.FullName;
                                loggedUser.Email = user.Email;
                                loggedUser.IsAuthenticated = true;
                            }
                            else
                            {
                                loggedUser.IsAuthenticated = false;
                            }
                        }
                    }
                    catch { }
                });
                GoToMainViewRequested?.Invoke(this, loggedUser);

            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }
    }
}
