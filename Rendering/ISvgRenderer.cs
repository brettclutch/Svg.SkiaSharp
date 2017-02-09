using System;
using Svg.SkiaSharp;
using System.Collections.Generic;

namespace Svg
{
    public interface ISvgRenderer : IDisposable
    {
        float DpiY { get; }
        void DrawImage(Bitmap image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit);
        //TODO: This was 'Point' not 'PointF', I changed but not sure if it will cause problems. 
        void DrawImageUnscaled(Bitmap image, PointF location);
        void DrawPath(Pen pen, GraphicsPath path);
        void FillPath(IBrush brush, GraphicsPath path);
        ISvgBoundable GetBoundable();
        Region GetClip();
        ISvgBoundable PopBoundable();
        void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append);
        void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append);
        void SetBoundable(ISvgBoundable boundable);
        void SetClip(Region region, CombineMode combineMode = CombineMode.Replace);
        SmoothingMode SmoothingMode { get; set; }
        Matrix Transform { get; set; }
        void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append);


        //Note: Added from SkiaSharp Branch. Technically doesn't make sense so might have to go different approach. Need to look at each implementation to see what is missing.  
        void DrawText(string text, float x, float y, Pen pen);
        Graphics Graphics { get; }
        void FillBackground(Color color);
        IDictionary<string, object> Context { get; }
        IDisposable UsingContextVariable(string key, object variable);
    }
}
