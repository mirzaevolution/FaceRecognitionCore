using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer.Models
{
    public class PreviewImageModel: INotifyPropertyChanged
    {
        private byte[] _imageBytes;
        private BitmapSource _image;
        private MemoryStream _memoryStream;
        private string _resolution, _size;
        public string Resolution
        {
            get { return _resolution; }
            set
            {
                if (_resolution != value)
                {
                    _resolution = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Resolution)));
                }
            }
        }
        public string Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Size)));
                }
            }
        }
        public MemoryStream MemoryStream
        {
            get { return _memoryStream; }
            set
            {
                if (_memoryStream != value)
                {
                    _memoryStream = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemoryStream)));
                }
            }
        }

        public byte[] ImageBytes
        {
            get { return _imageBytes; }
            set
            {
                if (_imageBytes != value)
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
