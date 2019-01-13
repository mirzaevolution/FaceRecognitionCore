using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FaceRecognition.DataLayer.Models;
using FaceRecognition.GUILayer.Models;
using MirzaCryptoHelpers.Hashings;
using MirzaCryptoHelpers.Common;
using FaceRecognition.DataLayer.Context;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using FaceRecognition.GUILayer.Helpers;
using ThumbnailSharp;
namespace FaceRecognition.GUILayer.History
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoryModel> _histories;
        public ObservableCollection<HistoryModel> Histories
        {
            get
            {
                return _histories;
            }
            set
            {
                if (_histories != value)
                {
                    _histories = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Histories)));
                }
            }
        }
        public async void LoadData()
        {
            var id = Global.LoggedUser.ID;
            List<HistoryModel> list = new List<HistoryModel>();
            bool success = true;
            string error = string.Empty;
            await Task.Run(() =>
            {
                try
                {
                    using (CoreContext context = new CoreContext())
                    {
                        var user = context.Users.FirstOrDefault(x => x.ID == id);
                        if (user != null)
                        {
                            foreach (var item in user.Histories.ToList())
                            {
                                HistoryModel model = new HistoryModel
                                {
                                    ID = item.ID,
                                    CapturedImageBytes = item.CapturedImage,
                                    SampleImageBytes = item.SampleImage,
                                    UserID = item.UserID,
                                    DetectionTime = item.DetectionTime,
                                    FullName = item.User?.FullName,
                                    DateTimeString= item.DateTime.ToString("MM/dd/yyyy hh:mm tt"),
                                    Distance = item.Distance,
                                    Method = item.Method
                                    
                                };
                                var capturedThumbnail = new ThumbnailCreator().CreateThumbnailBytes(200, item.CapturedImage, Format.Png);
                                var sampleThumbnail = new ThumbnailCreator().CreateThumbnailBytes(200, item.SampleImage, Format.Png);
                                model.CapturedImage = BitmapReader.Read(capturedThumbnail);
                                model.SampleImage = BitmapReader.Read(sampleThumbnail);
                                list.Add(model);

                            }
                        }
                        else
                        {
                            error = "Current logged user is invalid";
                            success = false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    success = false;
                    error = ex.Message;
                }
            });
            if (success)
            {
                if (list.Count == 0)
                {

                    Histories = new ObservableCollection<HistoryModel>(list);
                    Information?.Invoke("No items in the list");
                }
                else
                {

                    Histories = new ObservableCollection<HistoryModel>(list);
                    Information?.Invoke("Data loaded successfully");
                }
            }
            else
            {
                Histories = new ObservableCollection<HistoryModel>();
                ErrorOccured?.Invoke(error);
                LogHelper.LogException(new string[] { error });
            }
        }
        public event Action<string> Information;
        public event Action<string> ErrorOccured;
        public event EventHandler BackToMainRequested;

        public RelayCommand BackToMainCommand { get; set; }
        public HistoryViewModel()
        {
            BackToMainCommand = new RelayCommand(BackToMainHandler);
        }

        private void BackToMainHandler()
        {
            BackToMainRequested?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
