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

namespace BallanceSkyboxViewer {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private double angle_fov = 180; //from 0 -> 180
        private double angle_flatRotate = 0; //from 0 -> 360

        private double degToRad(double deg) {
            return deg * Math.PI * 2 / 360;
        }

        private void calcLookAndUpDirection(out Vector3D lookDirection, out Vector3D upDirection) {
            double cache_xy = Math.Sin(degToRad(angle_fov));
            lookDirection = new Vector3D(cache_xy * Math.Cos(degToRad(angle_flatRotate)), cache_xy * Math.Sin(degToRad(angle_flatRotate)), Math.Cos(degToRad(angle_fov)));

            cache_xy = Math.Sin(degToRad(angle_fov - 90));
            upDirection = new Vector3D(cache_xy * Math.Cos(degToRad(angle_flatRotate)), cache_xy * Math.Sin(degToRad(angle_flatRotate)), Math.Cos(degToRad(angle_fov - 90)));
        }


        #region event processor
        bool enableMove = false;
        bool initPre = true;
        Point prePosition = new Point(0, 0);
        double rotateSpeed = 0.3;

        private void Viewport3D_MouseDown(object sender, MouseButtonEventArgs e) {
            enableMove = true;
            initPre = true;
        }

        private void Viewport3D_MouseMove(object sender, MouseEventArgs e) {
            if (!enableMove) return;
            if (initPre) {
                var cache = e.GetPosition(this.uiDisplay);
                prePosition.X = cache.X;
                prePosition.Y = cache.Y;
                initPre = false;
                return;
            }

            //judge and limit number
            var cache2 = e.GetPosition(this.uiDisplay);
            var offset = cache2 - prePosition;

            double simulation_fov = angle_fov - offset.Y * rotateSpeed, simulation_rotate = angle_flatRotate + offset.X * rotateSpeed;
            while (simulation_rotate > 360)
                simulation_rotate -= 360;
            while (simulation_rotate < 0)
                simulation_rotate += 360;

            if (simulation_fov > 180) simulation_fov = 180;
            if (simulation_fov < 0) simulation_fov = 0;

            //write angle
            angle_flatRotate = simulation_rotate;
            angle_fov = simulation_fov;

            //update pre position
            prePosition.X = cache2.X;
            prePosition.Y = cache2.Y;

            //update ui
            calcLookAndUpDirection(out Vector3D lookDirc, out Vector3D upDirc);
            this.uiCamera.LookDirection = lookDirc;
            this.uiCamera.UpDirection = upDirc;
        }

        private void Viewport3D_MouseUp(object sender, MouseButtonEventArgs e) {
            enableMove = false;
        }


        #endregion

        //back down front left right
        string[] fileArray = new string[5] {
            "", "", "", "", ""
        };

        private void updateFileTooltip() {
            uiDragBack.ToolTip = fileArray[0];
            uiDragDown.ToolTip = fileArray[1];
            uiDragFront.ToolTip = fileArray[2];
            uiDragLeft.ToolTip = fileArray[3];
            uiDragRight.ToolTip = fileArray[4];
        }

        private void func_fileDrop(object sender, DragEventArgs e) {
            var cache = (System.Array)e.Data.GetData(DataFormats.FileDrop);
            if (cache.Length == 0) return;
            var file = cache.GetValue(0).ToString();

            switch (((Border)sender).Name) {
                case "uiDragBack":
                    fileArray[0] = file;
                    break;
                case "uiDragDown":
                    fileArray[1] = file;
                    break;
                case "uiDragFront":
                    fileArray[2] = file;
                    break;
                case "uiDragLeft":
                    fileArray[3] = file;
                    break;
                case "uiDragRight":
                    fileArray[4] = file;
                    break;
            }

            updateFileTooltip();

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 5; i++) {
                if (fileArray[i] == "") {
                    MessageBox.Show("Empty file!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            BitmapImage loadImage(Uri a) {
                var cache = new BitmapImage();
                cache.BeginInit();
                cache.CacheOption = BitmapCacheOption.OnLoad;
                cache.UriSource = a;
                cache.EndInit();
                return cache;
            }

            try {
                uiBackBrush.ImageSource = loadImage(new Uri(fileArray[0], UriKind.Absolute));
                uiBottomBrush.ImageSource = loadImage(new Uri(fileArray[1], UriKind.Absolute));
                uiFrontBrush.ImageSource = loadImage(new Uri(fileArray[2], UriKind.Absolute));
                uiLeftBrush.ImageSource = loadImage(new Uri(fileArray[3], UriKind.Absolute));
                uiRightBrush.ImageSource = loadImage(new Uri(fileArray[4], UriKind.Absolute));

                MessageBox.Show("Done");
            } catch (Exception ee) {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
