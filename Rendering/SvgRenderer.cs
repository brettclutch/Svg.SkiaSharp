﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Svg.SkiaSharp;

namespace Svg
{
    /// <summary>
    /// Convenience wrapper around a graphics object
    /// </summary>
    public sealed class SvgRenderer : IDisposable, IGraphicsProvider, ISvgRenderer
    {
        private Graphics _innerGraphics;
        private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();

        public void SetBoundable(ISvgBoundable boundable)
        {
            _boundables.Push(boundable);
        }
        public ISvgBoundable GetBoundable()
        {
            return _boundables.Peek();
        }
        public ISvgBoundable PopBoundable()
        {
            return _boundables.Pop();
        }

        public float DpiY
        {
            get { return _innerGraphics.DpiY; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ISvgRenderer"/> class.
        /// </summary>
        private SvgRenderer(Graphics graphics)
        {
            this._innerGraphics = graphics;
        }

        public void DrawImage(Bitmap image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
        {
            _innerGraphics.DrawImage(image, destRect, srcRect, graphicsUnit);
        }
        public void DrawImageUnscaled(Bitmap image, PointF location)
        {
            this._innerGraphics.DrawImageUnscaled(image, location);
        }
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            this._innerGraphics.DrawPath(pen, path);
        }
        public void FillPath(IBrush brush, GraphicsPath path)
        {
            this._innerGraphics.FillPath(brush, path);
        }
        public Region GetClip()
        {
            return this._innerGraphics.Clip;
        }
        public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.RotateTransform(fAngle, order);
        }
        public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.ScaleTransform(sx, sy, order);
        }
        public void SetClip(Region region, CombineMode combineMode = CombineMode.Replace)
        {
            this._innerGraphics.SetClip(region, combineMode);
        }
        public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
        {
            this._innerGraphics.TranslateTransform(dx, dy, order);
        }
        


        public SmoothingMode SmoothingMode
        {
            get { return this._innerGraphics.SmoothingMode; }
            set { this._innerGraphics.SmoothingMode = value; }
        }

        public Matrix Transform
        {
            get { return this._innerGraphics.Transform; }
            set { this._innerGraphics.Transform = value; }
        }

        public void Dispose()
        {
            this._innerGraphics.Dispose();
        }

        Graphics IGraphicsProvider.GetGraphics()
        {
            return _innerGraphics;
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="image"><see cref="Bitmap"/> from which to create the new <see cref="ISvgRenderer"/>.</param>
        public static ISvgRenderer FromImage(Bitmap image)
        {
            var g = new Graphics(image);

            return new SvgRenderer(g);
        }

        /// <summary>
        /// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="Graphics"/>.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> to create the renderer from.</param>
        public static ISvgRenderer FromGraphics(Graphics graphics)
        {
            return new SvgRenderer(graphics);
        }

        public static ISvgRenderer FromNull()
        {
            var img = new Bitmap(1, 1);
            return SvgRenderer.FromImage(img);
        }


        //Note: Items below added
        public void DrawText(string text, float x, float y, Pen pen)
        {
            this._innerGraphics.DrawText(text, x, y, pen);
        }

        public Graphics Graphics => _innerGraphics;

        public void FillBackground(Color color)
        {
            if (color == null) throw new ArgumentNullException(nameof(color));
            this._innerGraphics.FillBackground(color);
        }

        //TODO: Brett What is all this context stuff used for? Added it from SkiaSharp branch. Don't know what references are there. 
        private readonly IDictionary<string, object> _context = new Dictionary<string, object>();
        public IDictionary<string, object> Context => _context;

        public IDisposable UsingContextVariable(string key, object variable)
        {
            return new TemporaryContextVariable(key, variable, _context);
        }

        private class TemporaryContextVariable : IDisposable
        {
            private readonly string _key;
            private readonly object _variable;
            private IDictionary<string, object> _context;

            public TemporaryContextVariable(string key, object variable, IDictionary<string, object> context)
            {
                this._key = key;
                this._variable = variable;
                this._context = context;

                context[key] = variable;
            }


            public void Dispose()
            {
                _context.Remove(_key);
                (this._variable as IDisposable)?.Dispose();
            }
        }
    }
}