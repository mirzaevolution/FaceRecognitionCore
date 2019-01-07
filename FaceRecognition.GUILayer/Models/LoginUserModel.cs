using PropertyChanged;
namespace FaceRecognition.GUILayer.Models
{

    [AddINotifyPropertyChangedInterface]
    public class LoginUserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
