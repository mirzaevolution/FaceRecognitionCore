using PropertyChanged;
namespace FaceRecognition.DataLayer.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Repository
    {
        public int ID { get; set; }
        public byte[] SampleImage { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public virtual History History { get; set; }
    }
}
