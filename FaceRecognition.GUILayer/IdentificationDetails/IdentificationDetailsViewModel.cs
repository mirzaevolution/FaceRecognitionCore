using System.ComponentModel;
using FaceRecognition.GUILayer.Models;

namespace FaceRecognition.GUILayer.IdentificationDetails
{
    public class IdentificationDetailsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RepoResultModel _eigenfaceResult, _fisherfaceResult;
        private PreviewImageModel _previewImage;
        public PreviewImageModel PreviewImage
        {
            get { return _previewImage; }
            set
            {
                if (_previewImage != value)
                {
                    _previewImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviewImage)));
                }
            }
        }
      
        public RepoResultModel EigenfaceResult
        {
            get { return _eigenfaceResult; }
            set
            {
                if (_eigenfaceResult != value)
                {
                    _eigenfaceResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EigenfaceResult)));
                }
            }
        }
        public RepoResultModel FisherfaceResult
        {
            get { return _fisherfaceResult; }
            set
            {
                if (_fisherfaceResult != value)
                {
                    _fisherfaceResult = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FisherfaceResult)));
                }
            }
        }
        public IdentificationDetailsViewModel() { }
        public IdentificationDetailsViewModel
                (PreviewImageModel previewImage, 
                 RepoResultModel eigenfaceResult,
                 RepoResultModel fisherfaceResult)
        {
            this.PreviewImage = previewImage;
            this.EigenfaceResult = eigenfaceResult;
            this.FisherfaceResult = fisherfaceResult;
        }
    }
}
