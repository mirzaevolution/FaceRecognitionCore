
using System.ComponentModel;

namespace FaceRecognition.GUILayer.Models
{
    public class LoginUserModel:INotifyPropertyChanged
    {
        private string _username, _password;

        public string UserName
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                }
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
