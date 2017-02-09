using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg.SkiaSharp;

namespace Svg
{
    public interface IGraphicsProvider
    {
        Graphics GetGraphics();
    }
}
