
using Svg.SkiaSharp;
using SkiaSharp;
using System;

namespace Svg.SkiaSharp
{
    public class PathGradientBrush : BrushBase
    {
        GraphicsPath path { get; set; }
        public PointF CenterPoint { get; set; }
        public ColorBlend InterpolationColors { get; set; }

        public PathGradientBrush(GraphicsPath path)
        {
            throw new NotImplementedException();
        }

        protected override SKPaint CreatePaint()
        {
            throw new NotImplementedException();
        }
    }
}