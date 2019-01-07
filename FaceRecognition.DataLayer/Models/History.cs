using System.ComponentModel;
namespace FaceRecognition.DataLayer.Models
{
    
    public class History:INotifyPropertyChanged
    {
        private int _id;
        private byte[] _capturedImage;
        private byte[] _sampleImage;
        private int? _userId;
        private int? _repoId;
        private User _user;

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
        public byte[] SampleImage
        {
            get { return _sampleImage; }
            set
            {
                if (_sampleImage != value)
                {
                    _sampleImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleImage)));
                }
            }
        }
        public int? UserID
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
