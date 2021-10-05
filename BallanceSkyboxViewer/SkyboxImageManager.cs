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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace BallanceSkyboxViewer {
    public class SkyboxImageManager {
        public SkyboxImageManager() {
            btn = new BitmapImage[5];
            threeD = new BitmapImage[5];
            path = new string[5];

            for(int i = 0; i < 5;i++) {
                btn[i] = null;
                threeD[i] = null;
                path[i] = "";
            }
        }

        public BitmapImage BtnBack { get { return btn[0]; } }
        public BitmapImage BtnDown { get { return btn[1]; } }
        public BitmapImage BtnFront { get { return btn[2]; } }
        public BitmapImage BtnLeft { get { return btn[3]; } }
        public BitmapImage BtnRight { get { return btn[4]; } }

        public BitmapImage ThreeDBack { get { return threeD[0]; } }
        public BitmapImage ThreeDDown { get { return threeD[1]; } }
        public BitmapImage ThreeDFront { get { return threeD[2]; } }
        public BitmapImage ThreeDLeft { get { return threeD[3]; } }
        public BitmapImage ThreeDRight { get { return threeD[4]; } }

        public string FilePathBack { get { return path[0]; } }
        public string FilePathDown { get { return path[1]; } }
        public string FilePathFront { get { return path[2]; } }
        public string FilePathLeft { get { return path[3]; } }
        public string FilePathRight { get { return path[4]; } }

        BitmapImage[] btn, threeD;
        string[] path;

        public void LoadToBtn(string url, SkyboxFace face) {
            int i = (int)face;
            path[i] = url;
            btn[i] = loadImage(url);
        }

        public void ReloadToThreeD() {
            for(int i = 0; i < 5; i++) {
                if (path[i] == "") throw new Exception("No image!");
                btn[i] = loadImage(path[i]);
                threeD[i] = btn[i];
            }
        }

        // resources safe loader
        // https://stackoverflow.com/questions/8352787/how-to-free-the-memory-after-the-bitmapimage-is-no-longer-needed
        private BitmapImage loadImage(string url) {
            var bitmap = new BitmapImage();
            var stream = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.Read);

            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            stream.Close();
            stream.Dispose();
            bitmap.Freeze();

            return bitmap;
        }

    }

    public enum SkyboxFace : int {
        Back = 0,
        Down = 1,
        Front = 2,
        Left = 3,
        Right = 4
    }
}
