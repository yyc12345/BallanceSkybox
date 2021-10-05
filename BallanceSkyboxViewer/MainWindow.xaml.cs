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

            imageManager = new SkyboxImageManager();
        }

        #region camera processor

        private double degToRad(double deg) {
            return deg * Math.PI * 2 / 360;
        }

        private void calcLookAndUpDirection(out Vector3D lookDirection, out Vector3D upDirection) {
            double cache_xy = Math.Sin(degToRad(tilt_angle));
            lookDirection = new Vector3D(cache_xy * Math.Cos(degToRad(heading_angle)), cache_xy * Math.Sin(degToRad(heading_angle)), Math.Cos(degToRad(tilt_angle)));

            cache_xy = Math.Sin(degToRad(tilt_angle - 90));
            upDirection = new Vector3D(cache_xy * Math.Cos(degToRad(heading_angle)), cache_xy * Math.Sin(degToRad(heading_angle)), Math.Cos(degToRad(tilt_angle - 90)));
        }

        double tilt_angle = 180; //from 0 -> 180
        double heading_angle = 0; //from 0 -> 360
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

            double simulation_fov = tilt_angle - offset.Y * rotateSpeed, simulation_rotate = heading_angle + offset.X * rotateSpeed;
            while (simulation_rotate > 360)
                simulation_rotate -= 360;
            while (simulation_rotate < 0)
                simulation_rotate += 360;

            if (simulation_fov > 180) simulation_fov = 180;
            if (simulation_fov < 0) simulation_fov = 0;

            //write angle
            heading_angle = simulation_rotate;
            tilt_angle = simulation_fov;

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

        #region image processor

        private SkyboxImageManager imageManager;

        void updateBtnImage() {
            uiImgBtnBrush_Back.ImageSource = imageManager.BtnBack;
            uiImgBtnBrush_Down.ImageSource = imageManager.BtnDown;
            uiImgBtnBrush_Front.ImageSource = imageManager.BtnFront;
            uiImgBtnBrush_Left.ImageSource = imageManager.BtnLeft;
            uiImgBtnBrush_Right.ImageSource = imageManager.BtnRight;
        }

        void update3DImage() {
            ui3DBrush_Back.ImageSource = imageManager.ThreeDBack;
            ui3DBrush_Down.ImageSource = imageManager.ThreeDDown;
            ui3DBrush_Front.ImageSource = imageManager.ThreeDFront;
            ui3DBrush_Left.ImageSource = imageManager.ThreeDLeft;
            ui3DBrush_Right.ImageSource = imageManager.ThreeDRight;
        }

        void updateBtnTooltip() {
            uiImageBack.ToolTip = imageManager.FilePathBack;
            uiImageDown.ToolTip = imageManager.FilePathDown;
            uiImageFront.ToolTip = imageManager.FilePathFront;
            uiImageLeft.ToolTip = imageManager.FilePathLeft;
            uiImageRight.ToolTip = imageManager.FilePathRight;
        }

        private void func_fileOpen(object sender, MouseButtonEventArgs e) {
            var file = DialogUtils.OpenFileDialog();
            if (file == "") return;

            switch (((Border)sender).Name) {
                case nameof(uiImageBack):
                    imageManager.LoadToBtn(file, SkyboxFace.Back);
                    break;
                case nameof(uiImageDown):
                    imageManager.LoadToBtn(file, SkyboxFace.Down);
                    break;
                case nameof(uiImageFront):
                    imageManager.LoadToBtn(file, SkyboxFace.Front);
                    break;
                case nameof(uiImageLeft):
                    imageManager.LoadToBtn(file, SkyboxFace.Left);
                    break;
                case nameof(uiImageRight):
                    imageManager.LoadToBtn(file, SkyboxFace.Right);
                    break;
            }

            updateBtnImage();
            updateBtnTooltip();
        }

        private void func_fileDrop(object sender, DragEventArgs e) {
            var cache = (System.Array)e.Data.GetData(DataFormats.FileDrop);
            if (cache.Length == 1) {
                // only one file
                var file = cache.GetValue(0).ToString();

                switch (((Border)sender).Name) {
                    case nameof(uiImageBack):
                        imageManager.LoadToBtn(file, SkyboxFace.Back);
                        break;
                    case nameof(uiImageDown):
                        imageManager.LoadToBtn(file, SkyboxFace.Down);
                        break;
                    case nameof(uiImageFront):
                        imageManager.LoadToBtn(file, SkyboxFace.Front);
                        break;
                    case nameof(uiImageLeft):
                        imageManager.LoadToBtn(file, SkyboxFace.Left);
                        break;
                    case nameof(uiImageRight):
                        imageManager.LoadToBtn(file, SkyboxFace.Right);
                        break;
                }

            } else if (cache.Length == 5) {
                // 5 files, a series
                if (SkyboxFileMatch.IsLegal5Files(cache)) {
                    var f = SkyboxFileMatch.Get5Files(cache);
                    imageManager.LoadToBtn(f[0], SkyboxFace.Back);
                    imageManager.LoadToBtn(f[1], SkyboxFace.Down);
                    imageManager.LoadToBtn(f[2], SkyboxFace.Front);
                    imageManager.LoadToBtn(f[3], SkyboxFace.Left);
                    imageManager.LoadToBtn(f[4], SkyboxFace.Right);
                } else {
                    MessageBox.Show("Dropped bitmap series is illgal series.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } // skip

            updateBtnImage();
            updateBtnTooltip();
        }

        private void func_fileDropCheck(object sender, DragEventArgs e) {
            // only accept one file or 5 file
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                var arr = (System.Array)e.Data.GetData(DataFormats.FileDrop);
                if (arr.Length == 1 || arr.Length == 5) e.Effects = DragDropEffects.Link;
                else e.Effects = DragDropEffects.None;
            } else e.Effects = DragDropEffects.None;
        }

        private void func_Apply(object sender, RoutedEventArgs e) {
            try {
                imageManager.ReloadToThreeD();
                updateBtnImage();
                update3DImage();

                MessageBox.Show("Done");
            } catch (Exception ee) {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        #endregion

    }
}
