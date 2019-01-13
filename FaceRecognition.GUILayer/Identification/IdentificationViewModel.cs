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
using System.Windows.Media.Imaging;
using Accord.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;
using static Emgu.CV.Face.FaceRecognizer;

namespace FaceRecognition.GUILayer.Identification
{
    public class IdentificationViewModel : INotifyPropertyChanged
    {
        private EigenFaceRecognizer _eigenfaceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
        private FisherFaceRecognizer _fisherfaceRecognizer = new FisherFaceRecognizer(80, double.PositiveInfinity);


        private RepoResultModel _eigenfaceResult, _fisherfaceResult;
        private PreviewImageModel _previewImage, _previewImageCloned;
        private ObservableCollection<RepositoryModel> _repositories = new ObservableCollection<RepositoryModel>();
        private bool _isInProgress, _isCaptured;
        private double _noiseValue;
        public RelayCommand IdentifyCommand { get; set; }
        public RelayCommand SetNoiseCommand { get; set; }

        public RelayCommand CalculationDetailCommand { get; set; }
        public RelayCommand BackToMainCommand { get; set; }
                
        public bool IsInProgress
        {
            get
            {
                return _isInProgress;
            }
            set
            {
                if (_isInProgress != value)
                {

                    _isInProgress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInProgress)));
                }
            }
        }
        public bool IsCaptured
        {
            get
            {
                return _isCaptured;
            }
            set
            {
                if (_isCaptured != value)
                {

                    _isCaptured = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCaptured)));
                }
            }
        }

        public double NoiseValue
        {
            get
            {
                return _noiseValue;
            }
            set
            {
                if (_noiseValue != value)
                {

                    _noiseValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoiseValue)));
                }
            }
        }
        public PreviewImageModel PreviewImage
        {
            get { return _previewImage; }
            set
            {
                if(_previewImage!=value)
                {
                    _previewImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviewImage)));
                }
            }
        }
        public PreviewImageModel PreviewImageCloned
        {
            get { return _previewImageCloned; }
            set
            {
                if (_previewImageCloned != value)
                {
                    _previewImageCloned = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviewImageCloned)));
                }
            }
        }
        public RepoResultModel EigenfaceResult
        {
            get { return _eigenfaceResult; }
            set
            {
                if(_eigenfaceResult!=value)
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
        public ObservableCollection<RepositoryModel> Repositories
        {
            get { return _repositories; }
            set
            {
                if(_repositories!=value)
                {
                    _repositories = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Repositories)));
                }
            }
        }

        public event EventHandler BackToMainRequested;
        public event Action<string> Information;
        public event Action<string> ErrorOccured;
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<PreviewImageModel,RepoResultModel,RepoResultModel> IdentificationDetailsRequested;

        public IdentificationViewModel()
        {
            SetNoiseCommand = new RelayCommand(SetNoiseHandler, CanSetNoiseHandler);
            IdentifyCommand = new RelayCommand(IdentifyHandler, CanIdentifyHandler);
            CalculationDetailCommand = new RelayCommand(CalculateDetailHandler, CanCalculateDetailHandler);
            BackToMainCommand = new RelayCommand(BackToMainHandler, CanBackToMainHandler);
        }
        public async void LoadSamples()
        {

            Information?.Invoke("Getting all data from database...");
            IsInProgress = true;
            try
            {
                List<Repository> listFromDb = new List<Repository>();
                await Task.Run(() =>
                {

                    using (CoreContext context = new CoreContext())
                    {
                        listFromDb = context.Repositories.ToList();
                    }
                });
                if (listFromDb != null && listFromDb.Count > 0)
                {
                    var faceImages = new Image<Gray, byte>[listFromDb.Count];
                    var faceLabels = new int[listFromDb.Count];
                    int i = 0;
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
                        
                        faceImages[i] = new Image<Gray, byte>(new Bitmap(bitmap)).Resize(100, 100, Inter.Cubic);
                        faceLabels[i++] = model.ID;

                        Repositories.Add(model);
                    }
                    _eigenfaceRecognizer.Train(faceImages, faceLabels);
                    _fisherfaceRecognizer.Train(faceImages, faceLabels);
                    _eigenfaceRecognizer.Save("eigenface.txt");
                    _fisherfaceRecognizer.Save("fisherface.txt");

                    Information?.Invoke("Data loaded successfully.");


                }
                else
                {
                    Information?.Invoke("");
                    ErrorOccured?.Invoke("No data currently available from db. Please add visual training first!");
                }
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            finally
            {
                IsInProgress = false;
            }
        }
    
        private void SetNoiseHandler()
        {
            try
            {
                Information?.Invoke("Applying noise...");

                PreviewImage.Image = PreviewImageCloned.Image.Clone();
                PreviewImage.ImageBytes = PreviewImageCloned.ImageBytes.Clone() as byte[];
                MemoryStream ms = new MemoryStream();
                ms.Write(PreviewImage.ImageBytes, 0, PreviewImage.ImageBytes.Length);
                Bitmap bitmap = new Bitmap(ms);
                SaltAndPepperNoise filter = new SaltAndPepperNoise(NoiseValue);
                filter.ApplyInPlace(bitmap);
                var bitmapSource = BitmapConversion.BitmapToBitmapSource(bitmap);
                PreviewImage.Image = bitmapSource;
                MemoryStream ms2 = new MemoryStream();
                bitmap.Save(ms2, ImageFormat.Bmp);
                PreviewImage.ImageBytes = ms2.ToArray();
                ms.Close();
                ms2.Close();
                Information?.Invoke("Noise applied");
            }
            catch(Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }

        private bool CanSetNoiseHandler()
        {
            return !_isInProgress && PreviewImage != null;
        }

        private async void IdentifyHandler()
        {
            IsInProgress = true;
            bool success = true;
            string error = "";
            try
            {
                Information?.Invoke("Identifying provided face...");
                byte[] imageBytes = PreviewImage.ImageBytes;
                PredictionResult predictionResultEigenface = new PredictionResult();
                PredictionResult predictionResultFisherFace = new PredictionResult();
                string detectionTimeEigenface = string.Empty;
                string detectionTimeFisherface = string.Empty;
                var userID = Global.LoggedUser.ID;
                string distanceEigenface = string.Empty;
                string distanceFisherface = string.Empty;

                Image<Gray, byte> visual = null;
                try
                {
                    Stopwatch stopwatchEigenface = new Stopwatch();
                    Stopwatch stopwatchFisherface = new Stopwatch();

                    MemoryStream ms = new MemoryStream();
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    MemoryStream ms2 = new MemoryStream();
                    ms.CopyTo(ms2);
                    PreviewImage.MemoryStream = ms;
                    Bitmap bitmap = new Bitmap(ms);
                    visual = new Image<Gray, byte>(bitmap).Resize(100, 100, Inter.Cubic);
                    PreviewImage.Resolution = $"{bitmap.Width} x {bitmap.Height}";
                    PreviewImage.Size = $"{ms.Length.ToString("N0")} bytes";

                    

                    stopwatchEigenface.Start();
                    predictionResultEigenface = _eigenfaceRecognizer.Predict(visual);
         
                    stopwatchEigenface.Stop();

                    stopwatchFisherface.Start();
                    predictionResultFisherFace = _fisherfaceRecognizer.Predict(visual);
                    stopwatchFisherface.Stop();


                    detectionTimeEigenface = $"Detection time: {stopwatchEigenface.Elapsed.Milliseconds} ms";
                    detectionTimeFisherface = $"Detection time: {stopwatchFisherface.Elapsed.Milliseconds} ms";

                    distanceEigenface = $"Distance: {predictionResultEigenface.Distance.ToString("N2")}";
                    distanceFisherface = $"Distance: {predictionResultFisherFace.Distance.ToString("N2")}";

                    RepositoryModel repoEigenface = Repositories.FirstOrDefault(x => x.ID == predictionResultEigenface.Label);
                    RepositoryModel repoFisherface = Repositories.FirstOrDefault(x => x.ID == predictionResultFisherFace.Label);
                    DateTime now = DateTime.Now;
                    if (repoEigenface != null)
                    {
                        if (predictionResultEigenface.Distance <= 3000)
                        {
                            
                            EigenfaceResult = new RepoResultModel()
                            {
                                DetectionTime = detectionTimeEigenface,
                                Description = repoEigenface.Description,
                                ID = repoEigenface.ID,
                                Image = repoEigenface.Image,
                                ImageBytes = repoEigenface.SampleImage,
                                Distance = distanceEigenface
                            };
                            MemoryStream eigenfaceMs = new MemoryStream();
                            eigenfaceMs.Write(EigenfaceResult.ImageBytes, 0, EigenfaceResult.ImageBytes.Length);
                            EigenfaceResult.MemoryStream = eigenfaceMs;
                            EigenfaceResult.Size = eigenfaceMs.Length + " bytes";
                            EigenfaceResult.Resolution = $"{EigenfaceResult.Image.Width} x {EigenfaceResult.Image.Height}";
                            

                            using (CoreContext context = new CoreContext())
                            {
                                var user = context.Users.FirstOrDefault(x => x.ID == repoEigenface.UserID);
                                if (user != null)
                                {
                                    EigenfaceResult.FullName = user.FullName;
                                }
                                user.Histories.Add(new DataLayer.Models.History
                                {
                                    CapturedImage = EigenfaceResult.ImageBytes,
                                    DateTime = now,
                                    DetectionTime = EigenfaceResult.DetectionTime,
                                    Distance = EigenfaceResult.Distance,
                                    SampleImage = PreviewImage.ImageBytes,
                                    UserID = userID,
                                    Method = "Eigenface"
                                });
                                await context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            EigenfaceResult = new RepoResultModel
                            {
                                DetectionTime = string.Empty,
                                Description = "Not Found",
                                ID = 0,
                                Distance = string.Empty,
                                FullName = "Not Found"
                            };
                            BitmapImage image = new BitmapImage(new Uri("/Resources/not-found.png", UriKind.Relative));
                            EigenfaceResult.Image = image;
                        }

                       
                    }
                    if(repoFisherface!=null)
                    {
                        if (predictionResultFisherFace.Distance <= 3000)
                        {
                            FisherfaceResult = new RepoResultModel()
                            {
                                DetectionTime = detectionTimeEigenface,
                                Description = repoFisherface.Description,
                                ID = repoFisherface.ID,
                                Image = repoFisherface.Image,
                                ImageBytes = repoFisherface.SampleImage,
                                Distance = distanceFisherface
                            };
                            MemoryStream fisherfaceMs = new MemoryStream();
                            fisherfaceMs.Write(FisherfaceResult.ImageBytes, 0, FisherfaceResult.ImageBytes.Length);
                            FisherfaceResult.MemoryStream = fisherfaceMs;
                            FisherfaceResult.Size = fisherfaceMs.Length + " bytes";
                            FisherfaceResult.Resolution = $"{FisherfaceResult.Image.Width} x {FisherfaceResult.Image.Height}";


                            using (CoreContext context = new CoreContext())
                            {
                                var user = context.Users.FirstOrDefault(x => x.ID == repoFisherface.UserID);
                                if (user != null)
                                {
                                    FisherfaceResult.FullName = user.FullName;
                                }
                                user.Histories.Add(new DataLayer.Models.History
                                {
                                    CapturedImage = FisherfaceResult.ImageBytes,
                                    DateTime = now,
                                    DetectionTime = FisherfaceResult.DetectionTime,
                                    Distance = FisherfaceResult.Distance,
                                    SampleImage = PreviewImage.ImageBytes,
                                    UserID = userID,
                                    Method = "Fisherface"
                                });
                                await context.SaveChangesAsync();
                            }

                        }
                        else
                        {
                            FisherfaceResult = new RepoResultModel
                            {
                                DetectionTime = string.Empty,
                                Description = "Not Found",
                                ID = 0,
                                Distance = string.Empty,
                                FullName = "Not Found"
                            };
                            BitmapImage image = new BitmapImage(new Uri("/Resources/not-found.png", UriKind.Relative));
                            FisherfaceResult.Image = image;
                        }
                    }

                }
                catch (Exception ex)
                {
                    success = false;
                    error = ex.Message;
                }
                if (success)
                {
                    Information?.Invoke("Finished identifying.");
                }
                else
                {
                    Information?.Invoke("Finished identifying with an error.");
                    ErrorOccured?.Invoke(error);
                }
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                LogHelper.LogException(new string[] { ex.ToString() });
            }
            finally
            {
                IsInProgress = false;
            }
        }

        private bool CanIdentifyHandler()
        {
            return !_isInProgress && IsCaptured;
        }

        private void CalculateDetailHandler()
        {
            IdentificationDetailsRequested?.Invoke(PreviewImage, EigenfaceResult, FisherfaceResult);
        }
        private bool CanCalculateDetailHandler()
        {
            return ((PreviewImage!=null) && (EigenfaceResult != null) && (FisherfaceResult != null) && !_isInProgress);
        }
        private void BackToMainHandler()
        {
            EigenfaceResult = null;
            FisherfaceResult = null;
            PreviewImage = null;
            PreviewImageCloned = null;
            BackToMainRequested?.Invoke(this, EventArgs.Empty);
        }
        private bool CanBackToMainHandler()
        {
            return !IsInProgress;
        }
       
    }
}
