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
        public event Action<string> Information;
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
        public async void LoadSamples()
        {

            IsEnabled = false;
            IsLoading = true;
            try
            {
                Information?.Invoke("Loading samples from database....");
                var id = Global.LoggedUser.ID;
                List<Repository> listFromDb = new List<DataLayer.Models.Repository>();
                
                await Task.Run(() =>
                {

                    using (CoreContext context = new CoreContext())
                    {
                        listFromDb = context.Repositories.Where(x => x.UserID == id).ToList();
                    }
                });
                if (listFromDb != null && listFromDb.Count > 0)
                {
                    foreach (var item in listFromDb)
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

                        Repositories.Add(model);
                    }

                }
                Information?.Invoke("Data loaded successfully.");
            }
            catch(Exception ex)
            {
                Information?.Invoke("An error occured.");
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
            Information?.Invoke("Invoking preview handler...");
            PreviewImageRequested?.Invoke(this, Repository.Image);
            Information?.Invoke(string.Empty);

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
                        Information?.Invoke("Image removed successfully");

                    }
                    else
                    {
                        Information?.Invoke("Selected image is not found in db");

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
            Information?.Invoke(string.Empty);
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
            Information?.Invoke(string.Empty);
            IsSaved = false;
        }

        private bool CanCancelHandler()
        {
            return Repository != null;
        }


        private async void Add()
        {
            var id = Global.LoggedUser.ID;
            RepositoryModel item = null;

            Repository repo = new DataLayer.Models.Repository()
            {
                SampleImage = Repository.SampleImage,
                UserID = Repository.UserID,
                Description = Repository.Description
            };
            Information?.Invoke("Adding image to database...");
            bool success = true;
            string error = "";
            BitmapImage image = BitmapReader.Read(Repository.SampleImage);
            Bitmap bitmap = BitmapConversion.BitmapImageToBitmap(image);
            BitmapSource bitmapSource = BitmapConversion.BitmapToBitmapSource(bitmap);
            await Task.Run(() =>
            {
                try
                {
                    using (CoreContext context = new CoreContext())
                    {
                        var user = context.Users.FirstOrDefault(x => x.ID == id);
                        if (user != null)
                        {
                            user.Repositories.Add(repo);
                            context.SaveChanges();

                            item = new RepositoryModel
                            {
                                ID = repo.ID,
                                UserID = repo.UserID,
                                SampleImage = repo.SampleImage,
                                Description = repo.Description,
                                Image = bitmapSource,
                            };
                        }
                    }
                }
                catch(Exception ex)
                {
                    error = ex.Message;
                    success = false;
                }
            });
           
            if (item != null)
            {

                Repositories.Add(item);
            }
            if (success)
            {
                Information?.Invoke("New image added successfully.");
            }
            else
            {
                Information?.Invoke("Failed to add image.");
                ErrorOccured?.Invoke(error);
            }
        }
        private async void Update()
        {
            var id = Repository.ID;
            string desc = Repository.Description;
            Information?.Invoke("Adding image to database...");
            bool success = true;
            string error = "";
            await Task.Run(() =>
            {
                try
                {
                    using (CoreContext context = new CoreContext())
                    {
                        var repo = context.Repositories.FirstOrDefault(x => x.ID == id);
                        if (repo != null)
                        {
                            repo.Description = desc;
                            context.SaveChanges();
                        }

                    }

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    success = false;
                }
            });
            if (success)
            {
                Information?.Invoke("Data updated successfully.");
            }
            else
            {
                Information?.Invoke("");
                ErrorOccured?.Invoke(error);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
