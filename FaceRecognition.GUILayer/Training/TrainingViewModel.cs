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
using FaceRecognition.GUILayer.Helpers;
using System.Windows.Media.Imaging;

namespace FaceRecognition.GUILayer.Training
{

    public class TrainingViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RepositoryModel> _repositories;
        private RepositoryModel _repository = null;
        public event EventHandler BackToMainRequested;
        public ObservableCollection<RepositoryModel> Repositories
        {
            get { return _repositories; }
            set
            {
                if (_repositories!=value)
                {
                    _repositories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repositories)));
                }
            }
        }
        public RepositoryModel Repository
        {
            get { return _repository; }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repository)));
                }
            }
        }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand BackToMainCommand { get; set; }
        public async void LoadSamples()
        {
            try
            {
                var id = Global.LoggedUser.ID;
                List<RepositoryModel> list = new List<RepositoryModel>();
                await Task.Run(() =>
                {
                    using (CoreContext context = new CoreContext())
                    {
                        var result = context.Repositories.Where(x => x.UserID == id).ToList();
                        if (result != null && result.Count > 0)
                        {
                            foreach (var item in result)
                            {

                                RepositoryModel model = new RepositoryModel
                                {
                                    ID = item.ID,
                                    UserID = item.UserID,
                                    SampleImage = item.SampleImage
                                };

                                BitmapImage image = BitmapReader.Read(item.SampleImage);
                                Bitmap bitmap = BitmapConversion.BitmapImageToBitmap(image);
                                model.Image = BitmapConversion.BitmapToBitmapSource(bitmap);

                                list.Add(model);
                            }
                            Repositories = new ObservableCollection<RepositoryModel>(list);

                        }
                    }
                });
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }
        
        public TrainingViewModel()
        {
            Repositories = new ObservableCollection<RepositoryModel>();
            SaveCommand = new RelayCommand(SaveHandler, CanSaveHandler);
            CancelCommand = new RelayCommand(CancelHandler, CanCancelHandler);
            BackToMainCommand = new RelayCommand(BackToMainHandler);
        }

        private void BackToMainHandler()
        {
            BackToMainRequested?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveHandler()
        {
            try
            {
                var id = Global.LoggedUser.ID;
                RepositoryModel item = null;
                await Task.Run(() =>
                {
                    using(CoreContext context = new CoreContext())
                    {
                        Repository repo = new DataLayer.Models.Repository()
                        {
                            SampleImage = Repository.SampleImage,
                            UserID = Repository.UserID
                        };
                        var user = context.Users.FirstOrDefault(x => x.ID == id);
                        if (user != null)
                        {
                            user.Repositories.Add(repo);
                            context.SaveChanges();
                            BitmapImage image = BitmapReader.Read(Repository.SampleImage);
                            Bitmap bitmap = BitmapConversion.BitmapImageToBitmap(image);
                            item = new RepositoryModel
                            {
                                ID = repo.ID,
                                UserID = repo.UserID,
                                SampleImage = repo.SampleImage,
                                Image = BitmapConversion.BitmapToBitmapSource(bitmap),
                            };
                        }
                    }
                });
                if (item != null)
                {

                    Repositories.Add(item);
                    Repository = null;
                }
            }
            catch(Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private bool CanSaveHandler()
        {
            return Repository != null;
        }

        private void CancelHandler()
        {
            Repository = null;
        }

        private bool CanCancelHandler()
        {
            return Repository != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
