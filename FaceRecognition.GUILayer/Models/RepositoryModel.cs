using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer.Models
{
    public class RepositoryModel : INotifyPropertyChanged
    {
        private int _id;
        private byte[] _sampleImage;
        private int _userId;
        private string _description;
        private BitmapSource _image;
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
        public byte[] SampleImage
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
        public int UserID
        {
            get
            {
                return _userId;
            }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserID)));
                }
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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
