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
        private bool _isLoading, _isEnabled, _isCaptured, _isSaved;
        private RepositoryModel _repository = null;
        public event EventHandler BackToMainRequested;
        public event Action<string> ErrorOccured;
        public event EventHandler<BitmapSource> PreviewImageRequested;
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
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public bool IsCaptured
        {
            get { return _isCaptured; }
            set
            {
                if (_isCaptured != value)
                {
                    _isCaptured = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCaptured)));
                }
            }
        }
        public bool IsSaved
        {
            get { return _isSaved; }
            set
            {
                if (_isSaved != value)
                {
                    _isSaved = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSaved)));
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
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand PreviewImageCommand { get; set; }
        public RelayCommand BackToMainCommand { get; set; }
        public void LoadSamples()
        {

            IsEnabled = false;
            IsLoading = true;
            try
            {
                var id = Global.LoggedUser.ID;
                List<RepositoryModel> list = new List<RepositoryModel>();
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
                                Description = item.Description,
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
            }
            catch(Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            finally
            {
                IsEnabled = true;
                IsLoading = false;
            }
        }
        
        public TrainingViewModel()
        {
            IsEnabled = true;
            IsLoading = false;
            Repositories = new ObservableCollection<RepositoryModel>();
            SaveCommand = new RelayCommand(SaveHandler, CanSaveHandler);
            CancelCommand = new RelayCommand(CancelHandler, CanCancelHandler);
            DeleteCommand = new RelayCommand(DeleteHandler, CanDeleteHandler);
            PreviewImageCommand = new RelayCommand(PreviewImageHandler, CanPreviewImageHandler);
            BackToMainCommand = new RelayCommand(BackToMainHandler);
        }

        private bool CanPreviewImageHandler()
        {
            return Repository != null && !IsCaptured;

        }

        private void PreviewImageHandler()
        {
            PreviewImageRequested?.Invoke(this, Repository.Image);
        }

        private bool CanDeleteHandler()
        {
            return Repository != null && !IsCaptured;
        }

        private async void DeleteHandler()
        {
            IsEnabled = false;
            IsLoading = true;
            try
            {
                using(CoreContext context = new CoreContext())
                {
                    var image = context.Repositories.FirstOrDefault(x => x.ID == Repository.ID);
                    if (image != null)
                    {
                        context.Repositories.Remove(image);
                        await context.SaveChangesAsync();
                        var data = Repositories.FirstOrDefault(x => x.ID == Repository.ID);
                        if (data != null)
                        {
                            Repositories.Remove(data);
                        }
                        Repository = null;
                    }
                    else
                    {
                        ErrorOccured?.Invoke("Selected image is not found in db");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            finally
            {

                IsSaved = false;
                IsEnabled = true;
                IsLoading = false;
            }
        }

        private void BackToMainHandler()
        {
            Repository = null; 
            BackToMainRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SaveHandler()
        {
            IsEnabled = false;
            IsLoading = true;
            try
            {
                if(!IsSaved && IsCaptured)
                {
                    Add();
                }
                else
                {
                    Update();
                }
            }
            catch(Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            finally
            {
                Repository = null;
                IsCaptured = false;
                IsEnabled = true;
                IsLoading = false;
                IsSaved = false;
            }
        }

        private bool CanSaveHandler()
        {
            return Repository != null;
        }

        private void CancelHandler()
        {
            Repository = null;

            IsSaved = false;
        }

        private bool CanCancelHandler()
        {
            return Repository != null;
        }


        private void Add()
        {
            var id = Global.LoggedUser.ID;
            RepositoryModel item = null;
            using (CoreContext context = new CoreContext())
            {
                Repository repo = new DataLayer.Models.Repository()
                {
                    SampleImage = Repository.SampleImage,
                    UserID = Repository.UserID,
                    Description = Repository.Description
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
                        Description = repo.Description,
                        Image = BitmapConversion.BitmapToBitmapSource(bitmap),
                    };
                }
            }
            if (item != null)
            {

                Repositories.Add(item);
            }
        }
        private void Update()
        {
            var id = Global.LoggedUser.ID;
            using (CoreContext context = new CoreContext())
            {
                var repo = context.Repositories.FirstOrDefault(x => x.ID == Repository.ID);
                if (repo != null)
                {
                    repo.Description = Repository.Description;
                    context.SaveChanges();
                }
                
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
