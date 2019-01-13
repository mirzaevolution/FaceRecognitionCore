using System;
using System.ComponentModel;

namespace FaceRecognition.GUILayer.Home
{
    public class HomeViewModel:INotifyPropertyChanged
    {

        public event EventHandler GoToTrainingViewRequested;
        public event EventHandler GoToIdentificationViewRequested;
        public event EventHandler GoToHistoryViewRequested;
        public RelayCommand GoToTrainingCommand { get; set; }
        public RelayCommand GoToIdentificationCommand { get; set; }
        public RelayCommand GoToHistoryCommand { get; set; }


        public HomeViewModel()
        {
            GoToTrainingCommand = new RelayCommand(GoToTrainingHandler);
            GoToIdentificationCommand = new RelayCommand(GoToIdentificationHandler);
            GoToHistoryCommand = new RelayCommand(GoToHistoryHandler);
        }

        private void GoToHistoryHandler()
        {
            GoToHistoryViewRequested?.Invoke(this, EventArgs.Empty);
        }

        private void GoToIdentificationHandler()
        {
            GoToIdentificationViewRequested?.Invoke(this, EventArgs.Empty);
        }

        private void GoToTrainingHandler()
        {
            GoToTrainingViewRequested?.Invoke(this, EventArgs.Empty);

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
