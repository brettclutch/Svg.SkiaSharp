using Svg.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg.SkiaSharp
{
    public static class Colors
    {
        private static Color _black;
        private static Color _transparent;
        private static Color _white;


        public static Color Black => _black ?? (_black = new Color(0, 0, 0));
        public static Color Transparent => _transparent ?? (_transparent = new Color(0, 0, 0, 0));
        public static Color White => _white ?? (_white = new Color(255, 255, 255));
    }
}
