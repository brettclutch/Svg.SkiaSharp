using SkiaSharp;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Svg.SkiaSharp
{
    public class Color
    {
        private SKColor _inner;

        public Color()
        {
            _inner = new SKColor();
        }

        public Color(int alpha, Color color)
        {
            _inner = new SKColor((byte)color.R, (byte)color.G, (byte)color.B,  (byte)alpha);
        }

        public Color(int alpha, int r, int g, int b)
        {
            _inner = new SKColor((byte)r, (byte)g, (byte)b, (byte)alpha);
        }

        public Color(UInt32 integer)
        {
            _inner = new SKColor(integer);
        }

        public Color(string hex)
        {
            if (hex == null) throw new ArgumentException("Hex string cannot be null.", nameof(hex));

            if (Regex.IsMatch(hex.ToLowerInvariant(), @"^#[a-f0-9]{8}$"))
            {
                var a = int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
                var r = int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
                var g = int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
                var b = int.Parse(hex.Substring(7, 2), NumberStyles.HexNumber);

                _inner = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            }

            else if (Regex.IsMatch(hex.ToLowerInvariant(), @"^#[a-f0-9]{6}$"))
            {
                var r = int.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
                var g = int.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
                var b = int.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);

                _inner = new SKColor((byte)r, (byte)g, (byte)b, (byte)255);
            }
            else
                throw new ArgumentException("Not a valid hex string.", nameof(hex));

        }

        public Color(byte r, byte g, byte b)
        {
            _inner = new SKColor(r, g, b, 255);
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            _inner = new SKColor(r, g, b, a);
        }


        public Color(byte a, Color baseColor)
        {
            var b = ((Color)baseColor)._inner;
            _inner = new SKColor(b.Red, b.Green, b.Blue, a);
        }

        public Color(SKColor inner)
        {
            _inner = inner;
        }

        public string Name => "none";
        public bool IsKnownColor => false;
        public bool IsSystemColor => false;
        public bool IsNamedColor => false;
        public bool IsEmpty
        {
            get
            {
                return (SKColor.Empty == _inner);
            }
        }
        public byte A => _inner.Alpha;
        public byte R => _inner.Red;
        public byte G => _inner.Green;
        public byte B => _inner.Blue;
        public float GetBrightness()
        {
            throw new NotImplementedException();
        }
        public float GetSaturation()
        {
            throw new NotImplementedException();
        }
        public float GetHue()
        {
            throw new NotImplementedException();
        }
        public int ToArgb()
        {
            return B + G * 0x100 + R * 0x10000 + A * 0x1000000;
        }

        public bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public int GetHashCode()
        {
            return ToArgb();
        }

        public static Color Empty
        {
            get
            {
                return SKColor.Empty;
            }
        }

        public static implicit operator Color(SKColor other)
        {
            return new Color(other);
        }

        public static implicit operator SKColor(Color other)
        {
            return other._inner;
        }
    }
}
