using Svg.SkiaSharp; 

namespace Svg
{
    public interface ISvgBoundable
    {
        PointF Location
        {
            get;
        }

        SizeF Size
        {
            get;
        }

        RectangleF Bounds
        {
            get;
        } 
    }
}