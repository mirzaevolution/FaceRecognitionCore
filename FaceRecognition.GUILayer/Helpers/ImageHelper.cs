using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace FaceRecognition.GUILayer.Helpers
{
    public class ImageHelper
    {
        public static void SaveToLocal(Bitmap bitmap, string saveFileName)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            if (string.IsNullOrEmpty(saveFileName))
                throw new ArgumentNullException(nameof(saveFileName));
            try
            {
                string fullDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Global.ImagePath);

                if (!Directory.Exists(fullDirectory))
                {
                    Directory.CreateDirectory(fullDirectory);
                }
                string name = Path.Combine(fullDirectory, saveFileName);
                bitmap.Save(name, ImageFormat.Png);

            }
            catch (Exception ex)
            {
                LogHelper.LogException(new string[] { ex.ToString() });
            }
        }
    }
}
