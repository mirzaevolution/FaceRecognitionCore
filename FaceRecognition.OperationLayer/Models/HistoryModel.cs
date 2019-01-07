
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.OperationLayer.Models
{
    public class HistoryModel
    {
        public int ID { get; set; }
        public byte[] CapturedImage { get; set; }
        public int UserID { get; set; }
        public int RepositoryID { get; set; }
    }
}
