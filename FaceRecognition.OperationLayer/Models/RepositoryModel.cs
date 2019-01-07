using System.ComponentModel;

namespace FaceRecognition.OperationLayer.Models
{
    public class RepositoryModel:INotifyPropertyChanged
    {
        private int _id, _userId;
        public byte[] _sampleImage;

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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
