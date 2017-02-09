using System;
using SkiaSharp;

namespace Svg.SkiaSharp
{
    public class Graphics : IDisposable
    {
        private readonly SKSurface _surface;
        private readonly SKCanvas _canvas;
        private Matrix _matrix;
        private Region _clip;

        public Graphics(Bitmap image)
        {
            IntPtr length;
            var b = image.Image;
            _surface = SKSurface.Create(b.Info, b.GetPixels(out length), b.RowBytes);
            _canvas = _surface.Canvas;
            _matrix = new Matrix(_canvas.TotalMatrix);
        }

        public Graphics(SKSurface surface)
        {
            _surface = surface;
            _canvas = _surface.Canvas;
            _matrix = new Matrix(_canvas.TotalMatrix);
        }

        public Graphics(SKCanvas canvas)
        {
            _canvas = canvas;
            _matrix = new Matrix(_canvas.TotalMatrix);
        }

        public float DpiY
        {
            get
            {
                return 96;
            }
        }

        public Region Clip { get { return _clip; } }
        
        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;

        public void DrawImage(Bitmap bitmap, RectangleF rectangle, int x, int y, int width, int height, GraphicsUnit pixel)
        {
            var img = (Bitmap) bitmap;
            _canvas.DrawBitmap(img.Image, new SKRect(x, y, x+width,y+height));
        }

        public void DrawImage(Bitmap bitmap, RectangleF rectangle, int x, int y, int width, int height, GraphicsUnit pixel, ImageAttributes attributes)
        {
            var img = (Bitmap)bitmap;
            _canvas.DrawBitmap(img.Image, new SKRect(x, y, x + width, y + height));
            //throw new NotImplementedException("ImageAttributes not implemented for now: see http://chiuki.github.io/android-shaders-filters/#/");
        }

        public void DrawImage(Bitmap bitmap, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            var img = (Bitmap) bitmap;

            var src = (SKRect)(RectangleF)srcRect;
            var dest = (SKRect)(RectangleF)destRect;

            _canvas.DrawBitmap(img.Image, src, dest, null);
        }

        public void DrawImageUnscaled(Bitmap image, PointF location)
        {
            var img = (Bitmap)image;
            _canvas.DrawBitmap(img.Image, (int)location.X, (int)location.Y, null);
        }

        public void DrawImage(Bitmap image, PointF location)
        {
            var img = (Bitmap)image;
            _canvas.DrawBitmap(img.Image, (int)location.X, (int)location.Y, null);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            var p = (GraphicsPath) path;
            var paint = (Pen)pen;
            paint.Paint.IsStroke = true;
            SetSmoothingMode(paint.Paint);
            
            _canvas.DrawPath(p.Path, paint.Paint);

            // little hack as android path does not support text!
            foreach (var text in p.Texts)
            {
                _canvas.DrawText(text.text, text.location.X, text.location.Y, paint.Paint);
            }
        }

        public void FillPath(IBrush brush, GraphicsPath path)
        {
            var p = (GraphicsPath)path;

            var b = (BrushBase) brush;
            
            b.Paint.IsStroke = false;
            SetSmoothingMode(b.Paint);
                
            _canvas.DrawPath(p.Path, b.Paint);
        }

        public void DrawText(string text, float x, float y, Pen pen)
        {
            if (text == null)
                return;
            SetSmoothingMode(pen.Paint);
            _canvas.DrawText(text, x, y, pen.Paint);
        }

        private void SetSmoothingMode(SKPaint paint)
        {
            switch (SmoothingMode)
            {
                case SmoothingMode.HighQuality:
                    paint.FilterQuality = SKFilterQuality.High;
                    break;
                case SmoothingMode.AntiAlias:
                    paint.IsAntialias = true;
                    break;
                case SmoothingMode.HighSpeed:
                    paint.FilterQuality = SKFilterQuality.Low;
                    break;
                case SmoothingMode.Invalid:
                case SmoothingMode.Default:
                case SmoothingMode.None:
                    paint.IsAntialias = false;
                    paint.FilterQuality = SKFilterQuality.Medium;
                    break;
            }
        }

        public void SetClip(Region region, CombineMode combineMode)
        {
            var op = SKRegionOperation.Union;
            switch (combineMode)
            {
                case CombineMode.Complement:
                    // TODO LX is this correct?
                    op = SKRegionOperation.ReverseDifference;
                    break;
                case CombineMode.Exclude:
                    // TODO LX is this correct?
                    op = SKRegionOperation.Difference;
                    break;
                case CombineMode.Intersect:
                    op = SKRegionOperation.Intersect;
                    break;
                case CombineMode.Replace:
                    op = SKRegionOperation.Replace;
                    break;
                case CombineMode.Union:
                    op = SKRegionOperation.Union;
                    break;
                case CombineMode.Xor:
                    op = SKRegionOperation.XOR;
                    break;
            }
            _clip = region;
            //if (region != null)
            //    _canvas.ClipRect((RectangleF) region.Rect, op);
            //else
            //{
            //    SKRect r = new SKRect();
            //    _canvas.GetClipBounds(ref r);
            //    _canvas.ClipRect(r, SKRegionOperation.Union);
            //}
        }

        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF rectangle, StringFormat format)
        {
            SKPaint paint = font._paint;
            SKRect measureRegion = new SKRect();
            paint.MeasureText(text, ref measureRegion);
            RectangleF rect = RectangleF.FromLTRB(measureRegion.Left, measureRegion.Top, measureRegion.Right, measureRegion.Bottom);
            return new[] { new Region(rect) };
        }

        public Matrix Transform
        {
            get { return (Matrix)_canvas.TotalMatrix; }
            set
            {
                _matrix = (Matrix)value;
                _canvas.SetMatrix(_matrix.SKMatrix);
            }
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            if (order == MatrixOrder.Append)
            {
                _canvas.Translate(dx, dy);             
            }
            else
            {
                _canvas.Translate(dx, dy);
            }
        }

        public void RotateTransform(float fAngle, MatrixOrder order)
        {
            if (order == MatrixOrder.Append)
            {
                _canvas.RotateDegrees(fAngle); 
            }
            else
            {
                _canvas.RotateDegrees(fAngle);
            }
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            if (order == MatrixOrder.Append)
            {
                _canvas.Scale(sx, sy);              
            }
            else
            {
                _canvas.Scale(sx, sy);
            }
        }

        public void Concat(Matrix matrix)
        {
            var m = ((Matrix) matrix).SKMatrix;
            _canvas.Concat(ref m);
        }

        public void FillBackground(Color color)
        {
            var c = (Color)color;
            _canvas.DrawColor(c);
        }

        public void Flush()
        {
            _canvas.Flush(); 
        }

        public void Save()
        {
            _canvas.Save();
        }

        public void Restore()
        {
            _canvas.Restore();
        }

        public void Dispose()
        {
            if (_surface != null)
                _surface.Dispose();
        }
    }
}