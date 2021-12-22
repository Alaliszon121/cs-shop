using System;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;


namespace JaneczkoSklep
{
    public class ImageDecoder
    {
        public string SaveAsBase64(string path)
        {
            return Convert.ToBase64String(File.ReadAllBytes(path));
        }
        public BitmapSource ImageRead(string s)
        {
            return GetImageStream(LoadImage(s));
        }
        private Image LoadImage(string s)
        {
            byte[] bytes = Convert.FromBase64String(s);

            Image image;

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
        private BitmapSource GetImageStream(Image myImage)
        {
            Bitmap bitmap = new Bitmap(myImage);
            var bmpPt = bitmap.GetHbitmap();

            BitmapSource bitmapSource =
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmpPt,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }

        [DllImport("gdi32.dll")]

        public static extern bool DeleteObject(IntPtr hObject);
    }
}