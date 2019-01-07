using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FaceRecognition.GUILayer.PreviewFull
{
    /// <summary>
    /// Interaction logic for PreviewView.xaml
    /// </summary>
    public partial class PreviewView : MetroWindow
    {
        public PreviewView()
        {
            InitializeComponent();
        }

        public void SetImage(BitmapSource bitmap)
        {
            ImageLoader.Source = bitmap;
        }
    }
}
