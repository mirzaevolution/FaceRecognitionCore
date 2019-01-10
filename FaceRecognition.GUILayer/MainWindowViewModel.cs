using FaceRecognition.GUILayer.Home;
using FaceRecognition.GUILayer.Identification;
using FaceRecognition.GUILayer.PreviewFull;
using FaceRecognition.GUILayer.Training;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private object _currentView;
        private TrainingViewModel _trainingViewModel = new TrainingViewModel();
        private IdentificationViewModel _identificationViewModel = new IdentificationViewModel();
        private HomeViewModel _homeViewModel = new HomeViewModel();
        public event EventHandler<Tuple<double, double>> ResizeScreenRequested;
        public event Action<string> ErrorOccured;
        
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
                }
            }
        }

        public MainWindowViewModel()
        {
            _homeViewModel.GoToIdentificationViewRequested += GoToIdentificationHandler;
            _homeViewModel.GoToTrainingViewRequested += GoToTrainingHandler;
            _trainingViewModel.BackToMainRequested += BackToMainHandler;
            _trainingViewModel.PreviewImageRequested += PreviewImageHandler;
            _trainingViewModel.ErrorOccured += ErrorInfoHandler;
            CurrentView = _homeViewModel;

        }

        private void ErrorInfoHandler(string error)
        {
            ErrorOccured?.Invoke(error);   
        }

        private void PreviewImageHandler(object sender, BitmapSource e)
        {
            PreviewView previewView = new PreviewView();
            previewView.SetImage(e);
            previewView.ShowDialog();
        }

        private void BackToMainHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(390.387, 400.807));
            CurrentView = _homeViewModel;

        }

        private void GoToTrainingHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(694.487, 586.88));

            CurrentView = _trainingViewModel;
        }

        private void GoToIdentificationHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(811.2, 660.275));
            CurrentView = _identificationViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
