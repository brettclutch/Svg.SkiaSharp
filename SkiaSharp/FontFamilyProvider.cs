using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Svg.SkiaSharp
{
    //http://stackoverflow.com/questions/3532397/how-to-retrieve-a-list-of-available-installed-fonts-in-android
    public static class FontFamilyProvider 
    {
        public static IEnumerable<FontFamily> Families
        {
            get
            {
                return new List<FontFamily>()
                {
                    new FontFamily(SKTypeface.FromFamilyName(string.Empty, SKTypefaceStyle.Normal), "Default"), GenericSerif, GenericSansSerif, GenericMonospace,
                };
            }
        }

        public static FontFamily GenericSerif { get { return new FontFamily(SKTypeface.FromFamilyName("Serif"), "Serif"); } }
        public static FontFamily GenericSansSerif { get { return new FontFamily(SKTypeface.FromFamilyName("SansSerif"), "SansSerif"); } }
        public static FontFamily GenericMonospace { get { return new FontFamily(SKTypeface.FromFamilyName("Monospace"), "Monospace"); } }

        //TODO: Should this be FontFamily?
        public static StringFormat GenericTypographic { get { return  new StringFormat(); } }
    }
}