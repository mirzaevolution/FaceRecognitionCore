using System.Collections.Generic;
using System.ComponentModel;

namespace FaceRecognition.DataLayer.Models
{
    
    public class User:INotifyPropertyChanged
    {
        private int _id;
        private string _userName, _fullName, _passwordHash, _email;
        private ICollection<Repository> _repositories = new List<Repository>();
        private ICollection<History> _histories = new List<History>();
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
                if (_userName != value)
                {
                    _userName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                }
            }
        }
        public string FullName
        {
            get
            {
                return _fullName;
            }
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
                if (_passwordHash != value)
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }
        public virtual ICollection<Repository> Repositories
        {
            get { return _repositories; }
            set
            {
                if (_repositories != value)
                {
                    _repositories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repositories)));
                }
            }
        }
        public virtual ICollection<History> Histories
        {
            get { return _histories; }
            set
            {
                if(_histories!=value)
                {
                    _histories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Histories)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
