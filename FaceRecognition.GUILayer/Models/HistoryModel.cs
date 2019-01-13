using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;

namespace FaceRecognition.GUILayer.Models
{
    public class HistoryModel:INotifyPropertyChanged
    {
        private int _id;
        private byte[] _capturedImageBytes;
        private byte[] _sampleImageBytes;
        private string _fullName,_detectionTime,_distance,_method, _dateTimeStr;
        private BitmapImage _capturedImage;
        private BitmapSource _sampleImage;
        private int? _userId;
        private int? _repoId;
        private DateTime _dateTime;
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTime)));
                }
            }
        }
        public string DateTimeString
        {
            get { return _dateTimeStr; }
            set
            {
                if (_dateTimeStr != value)
                {
                    _dateTimeStr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTimeString)));
                }
            }
        }
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
        public byte[] CapturedImageBytes
        {
            get { return _capturedImageBytes; }
            set
            {
                if (_capturedImageBytes != value)
                {
                    _capturedImageBytes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CapturedImageBytes)));
                }
            }
        }
        public byte[] SampleImageBytes
        {
            get { return _sampleImageBytes; }
            set
            {
                if (_sampleImageBytes != value)
                {
                    _sampleImageBytes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleImageBytes)));
                }
            }
        }
        public BitmapImage CapturedImage
        {
            get
            {
                return _capturedImage;
            }
            set
            {
                if (_capturedImage != value)
                {
                    _capturedImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CapturedImage)));
                }
            }
        }
        public BitmapSource SampleImage
        {
            get
            {
                return _sampleImage;
            }
            set
            {
                if (_sampleImage != value)
                {
                    _sampleImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleImage)));
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
        public int? UserID
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserID)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
