using FaceRecognition.GUILayer.Home;
using FaceRecognition.GUILayer.Identification;
using FaceRecognition.GUILayer.Training;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.GUILayer
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private object _currentView;
        private TrainingViewModel _trainingViewModel = new TrainingViewModel();
        private IdentificationViewModel _identificationViewModel = new IdentificationViewModel();
        private HomeViewModel _homeViewModel = new HomeViewModel();
        public event EventHandler<Tuple<double, double>> ResizeScreenRequested;

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
            CurrentView = _homeViewModel;

        }

        private void BackToMainHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(390.387, 400.807));
            CurrentView = _homeViewModel;

        }

        private void GoToTrainingHandler(object sender, EventArgs e)
        {
            ResizeScreenRequested?.Invoke(this, new Tuple<double, double>(848.4, 708));

            CurrentView = _trainingViewModel;
        }

        private void GoToIdentificationHandler(object sender, EventArgs e)
        {
            CurrentView = _identificationViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
