using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BallanceSkyboxGenerator {
    public static class Kernel {

        static readonly int BK_SIZE = 512;

        public static void ConvertKernel(string img_origin_str, string storage_folder) {
            //open origin file
            var img_origin = new Bitmap(img_origin_str);
            //open saved file
            var img_back = new Bitmap(storage_folder + "Sky_A_Back.BMP", BK_SIZE, BK_SIZE);
            var img_front = new Bitmap(storage_folder + "Sky_A_Front.BMP", BK_SIZE, BK_SIZE);
            var img_left = new Bitmap(storage_folder + "Sky_A_Left.BMP", BK_SIZE, BK_SIZE);
            var img_right = new Bitmap(storage_folder + "Sky_A_Right.BMP", BK_SIZE, BK_SIZE);
            var img_down = new Bitmap(storage_folder + "Sky_A_Down.BMP", BK_SIZE, BK_SIZE);

            int img_origin_width = img_origin.Width;
            int img_origin_height = img_origin.Height;
            double central_x = img_origin_width / 2.0;
            double central_y = img_origin_height / 2.0;
            double circle = Math.Min(central_x, central_y);

            double heading = 0;
            double tilt = 0;
            double shadow_x = 0;
            double shadow_y = 0;

            Vector basePos = new Vector(-1.0, -1.0, -1.0);
            Vector offset = new Vector(0, 0, 0);

            void ClearData() {
                heading = 0;
                tilt = 0;
                shadow_x = 0;
                shadow_y = 0;
                offset = new Vector(0, 0, 0);
            }

            //down
            for (int x = 0; x < BK_SIZE; x++) {
                for (int y = 0; y < BK_SIZE; y++) {
                    offset.Y = 2.0 * ((double)x / (double)BK_SIZE);
                    offset.X = 2.0 * ((double)y / (double)BK_SIZE);
                    CalcAngle(basePos + offset, ref heading, ref tilt);
                    CalcShadowPos(circle, heading, tilt, ref shadow_x, ref shadow_y);

                    shadow_x += central_x;
                    shadow_y += central_y;
                    ConfirmPos(ref shadow_x, ref shadow_y, img_origin_width, img_origin_height);
                    img_down.SetPixel(x, y, img_origin.GetPixel(shadow_x, shadow_y));
                }
            }

            //front
            ClearData();
            basePos = new Vector(-1.0, -1.0, 1.0);
            for (int x = 0; x < BK_SIZE; x++) {
                for (int y = 0; y < BK_SIZE; y++) {
                    offset.Y = 2.0 * ((double)x / (double)BK_SIZE);
                    offset.Z = -2.0 * ((double)y / (double)BK_SIZE);
                    CalcAngle(basePos + offset, ref heading, ref tilt);
                    CalcShadowPos(circle, heading, tilt, ref shadow_x, ref shadow_y);

                    shadow_x += central_x;
                    shadow_y += central_y;
                    ConfirmPos(ref shadow_x, ref shadow_y, img_origin_width, img_origin_height);
                    img_front.SetPixel(x, y, img_origin.GetPixel(shadow_x, shadow_y));
                }
            }

            //back
            ClearData();
            basePos = new Vector(1.0, 1.0, 1.0);
            for (int x = 0; x < BK_SIZE; x++) {
                for (int y = 0; y < BK_SIZE; y++) {
                    offset.Y = -2.0 * ((double)x / (double)BK_SIZE);
                    offset.Z = -2.0 * ((double)y / (double)BK_SIZE);
                    CalcAngle(basePos + offset, ref heading, ref tilt);
                    CalcShadowPos(circle, heading, tilt, ref shadow_x, ref shadow_y);

                    shadow_x += central_x;
                    shadow_y += central_y;
                    ConfirmPos(ref shadow_x, ref shadow_y, img_origin_width, img_origin_height);
                    img_back.SetPixel(x, y, img_origin.GetPixel(shadow_x, shadow_y));
                }
            }

            //left
            ClearData();
            basePos = new Vector(1.0, -1.0, 1.0);
            for (int x = 0; x < BK_SIZE; x++) {
                for (int y = 0; y < BK_SIZE; y++) {
                    offset.X = -2.0 * ((double)x / (double)BK_SIZE);
                    offset.Z = -2.0 * ((double)y / (double)BK_SIZE);
                    CalcAngle(basePos + offset, ref heading, ref tilt);
                    CalcShadowPos(circle, heading, tilt, ref shadow_x, ref shadow_y);

                    shadow_x += central_x;
                    shadow_y += central_y;
                    ConfirmPos(ref shadow_x, ref shadow_y, img_origin_width, img_origin_height);
                    img_left.SetPixel(x, y, img_origin.GetPixel(shadow_x, shadow_y));
                }
            }

            //right
            ClearData();
            basePos = new Vector(-1.0, 1.0, 1.0);
            for (int x = 0; x < BK_SIZE; x++) {
                for (int y = 0; y < BK_SIZE; y++) {
                    offset.X = 2.0 * ((double)x / (double)BK_SIZE);
                    offset.Z = -2.0 * ((double)y / (double)BK_SIZE);
                    CalcAngle(basePos + offset, ref heading, ref tilt);
                    CalcShadowPos(circle, heading, tilt, ref shadow_x, ref shadow_y);

                    shadow_x += central_x;
                    shadow_y += central_y;
                    ConfirmPos(ref shadow_x, ref shadow_y, img_origin_width, img_origin_height);
                    img_right.SetPixel(x, y, img_origin.GetPixel(shadow_x, shadow_y));
                }
            }

            //close file
            img_origin.Close();
            img_back.Close();
            img_front.Close();
            img_left.Close();
            img_right.Close();
            img_down.Close();
        }

        static void CalcAngle(Vector pos, ref double heading_angle, ref double tilt_angle) {
            if (pos.Z > 0) tilt_angle = Math.PI / 2 + Math.Atan(Math.Abs(pos.Z) / Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2)));
            else if (pos.Z < 0) tilt_angle = Math.Atan(Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2)) / Math.Abs(pos.Z));
            else tilt_angle = Math.PI / 2;

            if (pos.X > 0 && pos.Y > 0) {
                heading_angle = Math.Atan(Math.Abs(pos.Y) / Math.Abs(pos.X));
            } else if (pos.X < 0 && pos.Y > 0) {
                heading_angle = Math.PI / 2.0 +
                    Math.Atan(Math.Abs(pos.X) / Math.Abs(pos.Y));
            } else if (pos.X < 0 && pos.Y < 0) {
                heading_angle = Math.PI +
                    Math.Atan(Math.Abs(pos.Y) / Math.Abs(pos.X));
            } else if (pos.X > 0 && pos.Y < 0) {
                heading_angle = 3.0 * Math.PI / 2.0 +
                    Math.Atan(Math.Abs(pos.X) / Math.Abs(pos.Y));
            } else if (pos.X > 0 && pos.Y == 0) heading_angle = 0;
            else if (pos.X == 0 && pos.Y > 0) heading_angle = Math.PI / 2.0;
            else if (pos.X < 0 && pos.Y == 0) heading_angle = Math.PI;
            else if (pos.X == 0 && pos.Y < 0) heading_angle = 3.0 * Math.PI / 2.0;
            else heading_angle = 0;

            return;
        }

        static void CalcShadowPos(double full_radius, double heading_angle, double tilt_angle, ref double shadow_x, ref double shadow_y) {
            var percent_tilt = tilt_angle / (3 * Math.PI / 4);
            var radius = percent_tilt * full_radius;
            shadow_x = radius * Math.Cos(heading_angle);
            shadow_y = radius * Math.Sin(heading_angle);
        }

        static void ConfirmPos(ref double x, ref double y, int max_width, int max_height) {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > max_width - 1) x = max_width - 1;
            if (y > max_height - 1) y = max_height - 1;
        }

    }
}
