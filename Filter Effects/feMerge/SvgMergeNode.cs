using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Svg.SkiaSharp;

namespace Svg.FilterEffects
{

	[SvgElement("feMergeNode")]
    public class SvgMergeNode : SvgElement
    {
        [SvgAttribute("in")]
        public string Input
        {
            get { return this.Attributes.GetAttribute<string>("in"); }
            set { this.Attributes["in"] = value; }
        }

		public override SvgElement DeepCopy()
		{
			throw new NotImplementedException();
		}

    }
}