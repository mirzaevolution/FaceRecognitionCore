using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer.Models
{
    public class RepoResultModel : INotifyPropertyChanged
    {
        private int _id;
        private string _fullName, _description, _detectionTime;
        private byte[] _imageBytes;
        private BitmapSource _image;
        public int ID
        {
            get { return _id; }
            set
            {
                if(_id!=value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                }
            }
        }
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if(_fullName!=value)
                {
                    _fullName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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
        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set
            {
                if(_imageBytes!=value)
                {
                    _imageBytes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageBytes)));
                }
            }
        }
        public BitmapSource Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
