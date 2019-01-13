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
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FaceRecognition.GUILayer.Helpers;
namespace FaceRecognition.GUILayer.ChangeProfile
{
    public class ChangeProfileViewModel:INotifyPropertyChanged
    {
        private bool _isLoading,_isEnabled;
        private ChangeUserProfileModel _currentUser;
        public ChangeUserProfileModel CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentUser)));
                }
            }
        }
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> ErrorOccured;
        public event Action<string> Information;
        public event EventHandler ExitRequested;
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
        public ChangeProfileViewModel()
        {
            IsEnabled = true;
            UpdateCommand = new RelayCommand(UpdateHandler,CanUpdateHandler);
            CancelCommand = new RelayCommand(CancelHandler);
            CurrentUser = new ChangeUserProfileModel
            {
                ID = Global.LoggedUser.ID,
                FullName = Global.LoggedUser.FullName,
                UserName = Global.LoggedUser.UserName,
                Email = Global.LoggedUser.Email
            };
            CurrentUser.CurrentPassword = CurrentUser.Password = string.Empty;
        }

        private async void UpdateHandler()
        {
            bool success = true;
            string error = string.Empty;
            IsLoading = true;
            IsEnabled = false;
            var currentUser = CurrentUser;
            try
            {
                if(!string.IsNullOrEmpty(currentUser.CurrentPassword) && string.IsNullOrEmpty(currentUser.Password))
                {
                    error = "Password cannot be empty!";
                    success = false;
                }
                else if(!string.IsNullOrEmpty(currentUser.Password) && string.IsNullOrEmpty(currentUser.CurrentPassword))
                {
                    error = "You must input current password!";
                    success = false;
                }
                else if (!string.IsNullOrEmpty(currentUser.Password) && 
                    !string.IsNullOrEmpty(currentUser.CurrentPassword) && 
                    currentUser.Password.Trim().Equals(currentUser.CurrentPassword.Trim()))
                {
                    error = "New password cannot be the same with current password!";
                    success = false;
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        using (CoreContext context = new CoreContext())
                        {
                            User user = context.Users.FirstOrDefault(x => x.ID == currentUser.ID);
                            if (user != null)
                            {
                                if (user.PasswordHash != new SHA512Crypto().GetHashBase64String(currentUser.CurrentPassword))
                                {
                                    error = "Current password is invalid";
                                    success = false;
                                }
                                else
                                {
                                    user.FullName = currentUser.FullName;
                                    user.UserName = currentUser.UserName;
                                    user.Email = currentUser.Email;
                                    if( !string.IsNullOrEmpty(currentUser.Password) &&
                                        !string.IsNullOrEmpty(currentUser.CurrentPassword))
                                    {
                                        user.PasswordHash = new SHA512Crypto().GetHashBase64String(currentUser.Password);
                                    }
                                    await context.SaveChangesAsync();
                                    
                                }
                            }
                            else
                            {
                                error = "User not found";
                                success = false;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            IsLoading = false;
            IsEnabled = true;
            if (success)
            {
                Global.LoggedUser = new LoggedUserModel
                {
                    ID = CurrentUser.ID,
                    FullName = CurrentUser.FullName,
                    Email = CurrentUser.Email,
                    UserName = CurrentUser.UserName,
                    IsAuthenticated = true
                };
                Information?.Invoke("Profile updated successfully");
            }
            else
            {
                ErrorOccured?.Invoke(error);
            }
        }

        private bool CanUpdateHandler()
        {
            return (CurrentUser != null) &&
                   (!string.IsNullOrEmpty(CurrentUser.FullName)) &&
                   (!string.IsNullOrEmpty(CurrentUser.UserName)) &&
                   (!string.IsNullOrEmpty(CurrentUser.Email));
        }

        private void CancelHandler()
        {
            ExitRequested?.Invoke(this,EventArgs.Empty);
        }

    }
}
