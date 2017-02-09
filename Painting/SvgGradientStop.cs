using System;
using System.Collections.Generic;
using System.Text;
using Svg.SkiaSharp;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Represents a color stop in a gradient.
    /// </summary>
    [SvgElement("stop")]
    public class SvgGradientStop : SvgElement
    {
        private SvgUnit _offset;
        
        /// <summary>
        /// Gets or sets the offset, i.e. where the stop begins from the beginning, of the gradient stop.
        /// </summary>
        [SvgAttribute("offset")]
        public SvgUnit Offset
        {
            get { return this._offset; }
            set
            {
                SvgUnit unit = value;

                if (value.Type == SvgUnitType.Percentage)
                {
                    if (value.Value > 100)
                    {
                        unit = new SvgUnit(value.Type, 100);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }
                else if (value.Type == SvgUnitType.User)
                {
                    if (value.Value > 1)
                    {
                        unit = new SvgUnit(value.Type, 1);
                    }
                    else if (value.Value < 0)
                    {
                        unit = new SvgUnit(value.Type, 0);
                    }
                }

                this._offset = unit.ToPercentage();
            }
        }

        /// <summary>
        /// Gets or sets the color of the gradient stop.
        /// </summary>
        [SvgAttribute("stop-color")]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public override SvgPaintServer StopColor
        {
            get 
            {
                var direct = this.Attributes.GetAttribute<SvgPaintServer>("stop-color", SvgColorServer.NotSet);
                if (direct == SvgColorServer.Inherit) return this.Attributes["stop-color"] as SvgPaintServer ?? SvgColorServer.NotSet;
                return direct;
            }
            set { this.Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the gradient stop (0-1).
        /// </summary>
        [SvgAttribute("stop-opacity")]
        public override float Opacity
        {
            get { return (this.Attributes["stop-opacity"] == null) ? 1.0f : (float)this.Attributes["stop-opacity"]; }
            set { this.Attributes["stop-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        public SvgGradientStop()
        {
            this._offset = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGradientStop"/> class.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="color">The color.</param>
        public SvgGradientStop(SvgUnit offset, Color color)
        {
            this._offset = offset;
        }

        public Color GetColor(SvgElement parent)
        {
            var core = SvgDeferredPaintServer.TryGet<SvgColorServer>(this.StopColor, parent);
            if (core == null) throw new InvalidOperationException("Invalid paint server for gradient stop detected.");
            return core.Color;
        }

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgGradientStop>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgGradientStop;
			newObj.Offset = this.Offset;
			return newObj;
		}
    }
}