using SkiaSharp;
using Svg.Interfaces;
using System;
using System.Globalization;

namespace Svg.SkiaSharp
{
    public class SizeF : IEquatable<SizeF>
    {
        private float width, height;
        public static readonly SizeF Empty = new SizeF(0,0);

        public SizeF(PointF pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        public SizeF(SizeF size)
        {
            width = size.Width;
            height = size.Height;
        }

        public SizeF(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public static implicit operator SizeF(SKSize other)
        {
            return new SizeF(other.Width, other.Height);
        }
        public static implicit operator SKSize(SizeF other)
        {
            return new SKSize(other.Width, other.Height);
        }

        /// <summary>
        ///	Addition Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Addition of two SizeF structures.
        /// </remarks>

        public static SizeF operator +(SizeF sz1, SizeF sz2)
        {
            return new SizeF (sz1.Width + sz2.Width,
                      sz1.Height + sz2.Height);
        }

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator ==(SizeF sz1, SizeF sz2)
        {
            return ((sz1?.Width == sz2?.Width) &&
                (sz1?.Height == sz2?.Height));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator !=(SizeF sz1, SizeF sz2)
        {
            return ((sz1?.Width != sz2?.Width) ||
                (sz1?.Height != sz2?.Height));
        }

        /// <summary>
        ///	Subtraction Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Subtracts two SizeF structures.
        /// </remarks>

        public static SizeF operator -(SizeF sz1, SizeF sz2)
        {
            return new SizeF(sz1.Width - sz2.Width,
                      sz1.Height - sz2.Height);
        }

        /// <summary>
        ///	SizeF to PointF Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Returns a PointF based on the dimensions of a given 
        ///	SizeF. Requires explicit cast.
        /// </remarks>

        public static explicit operator PointF(SizeF size)
        {
            return new PointF(size.Width, size.Height);
        }

        /// <summary>
        ///	IsEmpty Property
        /// </summary>
        ///
        /// <remarks>
        ///	Indicates if both Width and Height are zero.
        /// </remarks>

        public bool IsEmpty
        {
            get
            {
                return ((width == 0.0) && (height == 0.0));
            }
        }

        /// <summary>
        ///	Width Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Width coordinate of the SizeF.
        /// </remarks>

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        ///	Height Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Height coordinate of the SizeF.
        /// </remarks>

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this SizeF and another SizeF.
        /// </remarks>

        public bool Equals(SizeF other)
        {
            return ((this.Width == other.Width) &&
                (this.Height == other.Height));
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this SizeF and another object.
        /// </remarks>

        public override bool Equals(object obj)
        {
            if (!(obj is SizeF))
                return false;

            return this.Equals((SizeF)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode()
        {
            return (int)width ^ (int)height;
        }

        public PointF ToPointF()
        {
            return new PointF(width, height);
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the SizeF as a string in coordinate notation.
        /// </remarks>

        public override string ToString()
        {
            return string.Format("{{Width={0}, Height={1}}}", width.ToString(CultureInfo.CurrentCulture),
                height.ToString(CultureInfo.CurrentCulture));
        }

    }
}