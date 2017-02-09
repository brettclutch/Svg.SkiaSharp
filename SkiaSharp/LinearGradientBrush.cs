using System;
using Svg.SkiaSharp;
using System.Linq;
using SkiaSharp;
using Svg.Interfaces;

namespace Svg.SkiaSharp
{
    public class LinearGradientBrush : BrushBase, IDisposable
    {
        private readonly PointF _start;
        private readonly PointF _end;
        private readonly SKColor _colorStart;
        private readonly SKColor _colorEnd;
        private SKShader _shader;

        public LinearGradientBrush(PointF start, PointF end, Color colorStart, Color colorEnd)
        {
            _start = start;
            _end = end;
            _colorStart = new SKColor(colorStart.R, colorStart.G, colorStart.B, colorStart.A);
            _colorEnd = new SKColor(colorEnd.R, colorEnd.G, colorEnd.B, colorEnd.A);
        }

        public ColorBlend InterpolationColors { get; set; }

        public WrapMode WrapMode { get; set; }
        
        protected override SKPaint CreatePaint()
        {
            var paint = new SKPaint();
            SKShaderTileMode tileMode = SKShaderTileMode.Clamp;
            switch (WrapMode)
            {
                case WrapMode.Clamp:
                    tileMode = SKShaderTileMode.Clamp;
                    break;
                case WrapMode.Tile:
                    tileMode = SKShaderTileMode.Repeat;
                    break;
                case WrapMode.TileFlipX:
                case WrapMode.TileFlipXY:
                case WrapMode.TileFlipY:
                    tileMode = SKShaderTileMode.Mirror;
                    break;
            }

            if(_shader != null)_shader.Dispose();

            if(InterpolationColors == null)
                _shader = SKShader.CreateLinearGradient(new SKPoint( _start.X, _start.Y), new SKPoint(_end.X, _end.Y), new [] { _colorStart, _colorEnd}, null,  tileMode);
            else
            {
                _shader = SKShader.CreateLinearGradient(new SKPoint(_start.X, _start.Y), new SKPoint(_end.X, _end.Y), InterpolationColors.Colors.Select(c => new SKColor(c.R, c.G, c.B, c.A)).ToArray(), InterpolationColors.Positions, tileMode);
            }

            paint.Shader = _shader;
            return paint;
        }

        public override void Dispose()
        {
            base.Dispose();
            _shader?.Dispose();
            _shader = null;
        }
    }
}