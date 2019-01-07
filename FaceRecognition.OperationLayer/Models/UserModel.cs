using System.ComponentModel;

namespace FaceRecognition.OperationLayer.Models
{
    public class UserModel:INotifyPropertyChanged
    {
        private int _id;
        public string _userName,
                      _fullName,
                      _passwordHash,
                      _email;

        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                if(_userName!=value)
                {
                    _userName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                }
            }
        }
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                }
            }
        }
        public string PasswordHash
        {
            get { return _passwordHash; }
            set
            {
                if (_passwordHash!= value)
                {
                    _passwordHash = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordHash)));
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_email)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
