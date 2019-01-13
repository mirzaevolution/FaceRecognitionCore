using System;
using System.ComponentModel;
namespace FaceRecognition.DataLayer.Models
{
    
    public class History:INotifyPropertyChanged
    {
        private int _id;
        private byte[] _capturedImage;
        private byte[] _sampleImage;
        private string _detectionTime, _distance,_method;
        private int? _userId;
        private int? _repoId;
        private DateTime _dateTime;
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
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(DateTime)));
                }
            }
        }
        public string DetectionTime
        {
            get { return _detectionTime; }
            set
            {
                if (_detectionTime != value)
                {
                    _detectionTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DetectionTime)));
                }
            }
        }
        public string Distance
        {
            get { return _distance; }
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Distance)));
                }
            }
        }
        public string Method
        {
            get { return _method; }
            set
            {
                if (_method != value)
                {
                    _method = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Method)));
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
