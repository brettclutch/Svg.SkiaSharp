using System;
using System.IO;
using SkiaSharp;

namespace Svg.SkiaSharp
{

    public class Bitmap : IDisposable
    {
        protected readonly SKBitmap _image;

        public Bitmap(int width, int height) : this(new SKBitmap(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Premul))
        {
        }

        public Bitmap(Bitmap inputImage) : this(new SKBitmap(((Bitmap) inputImage)._image.Info))
        {
        }

        public Bitmap(SKBitmap bitmap)
        {
            _image = bitmap;
            Width = _image.Width;
            Height = _image.Height;
        }

        protected Bitmap()
        {

        }

        public SKBitmap Image
        {
            get { return _image; }
        }

        public void Dispose()
        {
            _image.Dispose();
        }

        public BitmapData LockBits(RectangleF rectangle, ImageLockMode lockmode, PixelFormat pixelFormat)
        {
            throw new NotImplementedException();
        }

        public void UnlockBits(BitmapData bitmapData)
        {
            _image.UnlockPixels();
        }

        public void SavePng(Stream stream, int quality = 100)
        {
            using (var img = SKImage.FromBitmap(_image))
            {
                var data = img.Encode(SKImageEncodeFormat.Png, quality: quality);
                data.SaveTo(stream);
            }
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public static Bitmap FromStream(Stream stream)
        {
            using (var s = new SKManagedStream(stream))
            {
                var bm = SKBitmap.Decode(s);
                return new Bitmap(bm);
            }
        }
    }
}