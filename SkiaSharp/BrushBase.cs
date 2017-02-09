using System;
using SkiaSharp;

namespace Svg.SkiaSharp
{
    public abstract class BrushBase : IDisposable, IBrush
    {
        private SKPaint _paint;

        public SKPaint Paint
        {
            get
            {
                if (_paint == null)
                {
                    _paint = CreatePaint();
                    _paint.IsStroke = false;
                }
                return _paint;
            }
        }
        protected abstract SKPaint CreatePaint();

        public virtual void Dispose()
        {
            _paint?.Dispose();
            _paint = null;
        }
    }
}