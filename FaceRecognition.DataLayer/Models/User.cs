using System.Collections.Generic;
using PropertyChanged;
namespace FaceRecognition.DataLayer.Models
{
    [AddINotifyPropertyChangedInterface]
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Repository> Repositories { get; set; } =
            new List<Repository>();
        public virtual ICollection<History> Histories { get; set; }
    }
}
