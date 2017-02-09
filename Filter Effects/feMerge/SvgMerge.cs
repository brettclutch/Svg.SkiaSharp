using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Svg.SkiaSharp;
using System.Linq;
using SkiaSharp;

namespace Svg.FilterEffects
{
	[SvgElement("feMerge")]
    public class SvgMerge : SvgFilterPrimitive
    {
        public override void Process(ImageBuffer buffer)
        {
            var children = this.Children.OfType<SvgMergeNode>().ToList();
            var inputImage = buffer[children.First().Input];
            var result = new Bitmap(inputImage.Width, inputImage.Height);
            using (var g = new Graphics(result))
            {
                foreach (var child in children)
                {
                    g.DrawImage(buffer[child.Input], new RectangleF(0, 0, inputImage.Width, inputImage.Height),
                                0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);
                }
                g.Flush();
            }
            buffer[this.Result] = result;
        }

		public override SvgElement DeepCopy()
		{
            return DeepCopy<SvgMerge>();
		}

    }
}