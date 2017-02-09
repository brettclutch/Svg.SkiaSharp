using SkiaSharp;

namespace Svg.SkiaSharp
{
    public class SolidBrush : BrushBase
    {
        private readonly SKColor _color;
        private SKPaint _p;

        public SolidBrush(Color color)
        {
            _color = new SKColor(color.R, color.G, color.B, color.A);
        }
        public SolidBrush(SKPaint paint)
        {
            _p = paint;
        }

        protected override SKPaint CreatePaint()
        {
            if (_p != null)
                return _p;

            var paint = new SKPaint();
            paint.Color = _color;
            return paint;
        }
    }
}