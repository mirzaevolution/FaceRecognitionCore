using FaceRecognition.GUILayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.GUILayer
{
    public class Global
    {
        public static LoggedUserModel LoggedUser { get; set; }
        public static string UserFullName { get; set; }
        public static string ImagePath { get; set; } = "Images";
        public static string LogFileName { get; set; } = "data.log";
    }
}
