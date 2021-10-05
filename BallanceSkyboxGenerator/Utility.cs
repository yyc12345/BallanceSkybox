using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BallanceSkyboxGenerator {

    public struct Vector {
        public Vector(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
        }
        public double X;
        public double Y;
        public double Z;

        public static Vector operator +(Vector a, Vector b) {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
    }

    public struct ColorRGB {
        public ColorRGB(double r, double g, double b) {
            R = r;
            G = g;
            B = b;
        }
        public double R;
        public double G;
        public double B;

        public static ColorRGB operator +(ColorRGB a, ColorRGB b) {
            return new ColorRGB(a.R + b.R, a.G + b.G, a.B + b.B);
        }
        public static ColorRGB operator -(ColorRGB a, ColorRGB b) {
            return new ColorRGB(a.R - b.R, a.G - b.G, a.B - b.B);
        }
        public static ColorRGB operator *(ColorRGB a, double num) {
            return new ColorRGB(a.R * num, a.G * num, a.B * num);
        }
        public static ColorRGB operator /(ColorRGB a, double num) {
            return new ColorRGB(a.R / num, a.G / num, a.B / num);
        }
    }

    public class Bitmap {

        public Bitmap(string file) {
            isOpen = true;
            fileUrl = file;
            image = new System.Drawing.Bitmap(file);
            if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                throw new InvalidOperationException("Unsupport format. Please use 24bit RGB BMP image");
            realImage = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        public Bitmap(string file, int width, int height) {
            isOpen = false;
            fileUrl = file;
            image = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            realImage = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        bool isOpen;
        string fileUrl;
        System.Drawing.Bitmap image;
        System.Drawing.Imaging.BitmapData realImage;
        const int PIXEL_SIZE = 3;
        public int Width { get { return image.Width; } }
        public int Height { get { return image.Height; } }

        public void Close() {
            //unlock
            image.UnlockBits(realImage);
            //release
            if (!isOpen) image.Save(fileUrl, System.Drawing.Imaging.ImageFormat.Bmp);
            image.Dispose();
        }

        public ColorRGB GetPixel(double x, double y) {
            //return GetPixel((int)x, (int)y);
            int basex = (int)x, basey = (int)y;
            //if (basex == Width - 1 || basey == Height - 1) return GetPixel(basex, basey); // this if will cause sharp edge, we clamp it in GetPixel(int, int) to avoid this.

            var x_percentage = x - (double)basex;
            var y_percentage = y - (double)basey;

            var midCache1 = GetPixel(basex, basey) * (1 - x_percentage) + GetPixel(basex + 1, basey) * x_percentage;
            var midCache2 = GetPixel(basex, basey + 1) * (1 - x_percentage) + GetPixel(basex + 1, basey + 1) * x_percentage;

            return (midCache1 * (1 - y_percentage) + midCache2 * y_percentage);
        }

        public ColorRGB GetPixel(int x, int y) {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x >= Width) x = Width - 1;
            if (y >= Height) y = Height - 1;

            int offset = y * realImage.Stride + x * PIXEL_SIZE;
            //BGR -> RGB
            return new ColorRGB(
                System.Runtime.InteropServices.Marshal.ReadByte(realImage.Scan0, offset + 2),
                System.Runtime.InteropServices.Marshal.ReadByte(realImage.Scan0, offset + 1),
                System.Runtime.InteropServices.Marshal.ReadByte(realImage.Scan0, offset));
        }

        public void SetPixel(int x, int y, ColorRGB color) {
            int offset = y * realImage.Stride + x * PIXEL_SIZE;
            //RGB -> BGR
            System.Runtime.InteropServices.Marshal.WriteByte(realImage.Scan0, offset, (byte)color.B);
            System.Runtime.InteropServices.Marshal.WriteByte(realImage.Scan0, offset + 1, (byte)color.G);
            System.Runtime.InteropServices.Marshal.WriteByte(realImage.Scan0, offset + 2, (byte)color.R);
        }
    }

}
