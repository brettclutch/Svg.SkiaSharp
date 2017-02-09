using System;
using System.Collections.Generic;
using System.Text;
using Svg.SkiaSharp;

namespace Svg
{
    public sealed class SvgColorServer : SvgPaintServer
    {
    	
    	/// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer NotSet = new SvgColorServer(Colors.Black);
        /// <summary>
        /// A <see cref="SvgPaintServer"/> that should inherit from its parent.
        /// </summary>
        public static readonly SvgPaintServer Inherit = new SvgColorServer(Colors.Black);

        public SvgColorServer()
            : this(Colors.Black)
        {
        }

        public SvgColorServer(Color color)
        {
            this._colour = color;
        }

        //BB: Changed to Color.Empty
        private Color _colour = Color.Empty;

        public Color Color
        {
            get { return this._colour; }
            set { this._colour = value; }
        }

        public override IBrush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            //is none?
            if (this == SvgPaintServer.None) return new SolidBrush(Colors.Transparent);
                
            int alpha = (int)Math.Round((opacity * (this.Color.A/255.0) ) * 255);
            Color color = new Color(alpha, this.Color);

            return new SolidBrush(color);
        }

        public override string ToString()
        {
        	if(this == SvgPaintServer.None)
        		return "none";
        	else if(this == SvgColorServer.NotSet)
        		return "";
        	
            Color c = this.Color;

            // Return the name if it exists
            if (c.IsKnownColor)
            {
                return c.Name;
            }

            // Return the hex value
            return String.Format("#{0}", c.ToArgb().ToString("x").Substring(2));
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgColorServer>();
		}


		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgColorServer;
			newObj.Color = this.Color;
			return newObj;

		}

        public override bool Equals(object obj)
        {
            var objColor = obj as SvgColorServer;
            if (objColor == null)
                return false;

            if ((this == SvgPaintServer.None && obj != SvgPaintServer.None) ||
                (this != SvgPaintServer.None && obj == SvgPaintServer.None) ||
                (this == SvgColorServer.NotSet && obj != SvgColorServer.NotSet) ||
                (this != SvgColorServer.NotSet && obj == SvgColorServer.NotSet) ||
                (this == SvgColorServer.Inherit && obj != SvgColorServer.Inherit) ||
                (this != SvgColorServer.Inherit && obj == SvgColorServer.Inherit)) return false;

            return this.GetHashCode() == objColor.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _colour.GetHashCode();
        }
    }
}
