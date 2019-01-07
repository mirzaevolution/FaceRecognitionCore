using PropertyChanged;
namespace FaceRecognition.DataLayer.Models
{
    [AddINotifyPropertyChangedInterface]
    public class History
    {
        public int ID { get; set; }
        public byte[] CapturedImage { get; set; }
        public int UserID { get; set; }
        public int RepositoryID { get; set; }
        public virtual User User { get; set; }
        public virtual Repository Repository { get; set; }
    }
}
