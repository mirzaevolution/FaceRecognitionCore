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
using FaceRecognition.GUILayer.Models;

namespace FaceRecognition.GUILayer
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private object _currentView;
        private bool _isVisible;
        private TrainingViewModel _trainingViewModel = new TrainingViewModel();
        private IdentificationViewModel _identificationViewModel = new IdentificationViewModel();
        private HomeViewModel _homeViewModel = new HomeViewModel();
        public event EventHandler<Tuple<double, double>> ResizeScreenRequested;
        public event Action<string> ErrorOccured;
        public event Action<string> Information;
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
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible)));
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
            _trainingViewModel.Information += InformationHandler;
            _identificationViewModel.BackToMainRequested += BackToMainHandler;
            _identificationViewModel.ErrorOccured += ErrorInfoHandler;
            _identificationViewModel.Information += InformationHandler;
            CurrentView = _homeViewModel;
            IsVisible = false;
        }



        private void InformationHandler(string obj)
        {
            Information?.Invoke(obj);
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
            IsVisible = false;
            CurrentView = _homeViewModel;
        }

        private void GoToTrainingHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(694.487, 600));
            IsVisible = true;
            CurrentView = _trainingViewModel;
        }

        private void GoToIdentificationHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(811.2, 690.275));
            IsVisible = true;
            CurrentView = _identificationViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
