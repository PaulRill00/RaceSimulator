using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace Visuals
{
    public static class ImageLoader
    {
        private static Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();

        public static Bitmap GetBitmap(string url)
        {
            if (!_cache.ContainsKey(url))
                _cache.Add(url, new Bitmap(Image.FromFile(url)));
            return (Bitmap) _cache.GetValueOrDefault(url).Clone();
        }

        public static void ClearCache()
        {
            _cache.Clear();
        }

        public static Bitmap CreateBitmapFromSize(int width, int height)
        {
            Bitmap map = _cache.GetValueOrDefault("empty");

            if (map == null)
                map = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(map);
            SolidBrush brush = new SolidBrush(Color.FromArgb(255,23,168,115));
            g.FillRectangle(brush, 0, 0, width, height);

            if (!_cache.ContainsKey("empty"))
                _cache.Add("empty", map);

            return (Bitmap) map.Clone();
        }

        // copied method
        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
