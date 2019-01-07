using System.Windows.Media.Imaging;
using System.IO;

namespace FaceRecognition.GUILayer.Helpers
{
    public class BitmapReader
    {
        public static BitmapImage Read(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(byteArray))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
