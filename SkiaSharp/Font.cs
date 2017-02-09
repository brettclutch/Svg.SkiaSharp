using SkiaSharp;
using System;

namespace Svg.SkiaSharp
{
    public class Font : IDisposable
    {
        private FontStyle _style;
        private FontFamily _fontFamily;
        public SKPaint _paint;

        public Font(FontFamily fontFamily)
        {
            _fontFamily = fontFamily;
            _paint = new SKPaint();
            _paint.Typeface =  _fontFamily.Typeface;
        }

        //Note: BB Added
        public Font(FontFamily fontFamily, float size, FontStyle fontStyle, GraphicsUnit graphicsUnit)
        {
            _fontFamily = fontFamily;
            _paint = new SKPaint();
            _paint.TextSize = size;
            _style = fontStyle;
            _paint.Typeface = SKTypeface.FromTypeface(_fontFamily.Typeface, fontStyle.ToSKTypefaceStyle());

            //TODO: What to use graphics Unit for? 
        }

        public void Dispose()
        {
            if (_paint != null)
            {
                _paint.Dispose();
                _paint = null;
            }
        }

        public float Size
        {
            get { return _paint.TextSize; }
            set { _paint.TextSize = value; }
        }

        public float SizeInPoints
        {
            get { return _paint.TextSize; }
            set { _paint.TextSize = value; }
        }

        public FontStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                _paint.Typeface = SKTypeface.FromTypeface(_fontFamily.Typeface, value.ToSKTypefaceStyle());
            }
        }

        public FontFamily FontFamily { get { return _fontFamily; } }
    }
}