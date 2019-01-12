using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer.Models
{
    public class PreviewImageModel: INotifyPropertyChanged
    {
        private byte[] _imageBytes;
        private BitmapSource _image;
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
