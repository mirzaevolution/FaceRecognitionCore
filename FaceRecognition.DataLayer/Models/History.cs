using System.ComponentModel;
namespace FaceRecognition.DataLayer.Models
{
    
    public class History:INotifyPropertyChanged
    {
        private int _id;
        private byte[] _capturedImage;
        private int _userId;
        private int _repoId;
        private User _user;
        private Repository _repository;

        public int ID
        {
            get { return _id;  }
            set
            {
                if(_id!=value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }
        public byte[] CapturedImage
        {
            get { return _capturedImage; }
            set
            {
                if(_capturedImage!=value)
                {
                    _capturedImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CapturedImage)));
                }
            }
        }
        public int UserID
        {
            get { return _userId; }
            set
            {
                if(_userId!=value)
                {
                    _userId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserID)));
                }
            }
        }
        public int RepositoryID
        {
            get { return _repoId; }
            set
            {
                if (_repoId != value)
                {
                    _repoId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RepositoryID)));
                }
            }
        }
        public virtual User User
        {
            get { return _user; }
            set
            {
                if (_user!=value)
                {
                    _user = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
                }
            }
        }
        public virtual Repository Repository
        {
            get { return _repository; }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repository)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
