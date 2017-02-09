using System;
using Svg.SkiaSharp;

namespace Svg.SkiaSharp
{
    public class StringFormat : ICloneable
    {
        public StringFormat()
        {
        }

        public StringFormatFlags FormatFlags { get; set; }
        public void SetMeasurableCharacterRanges(CharacterRange[] characterRanges)
        {
            if (characterRanges == null) throw new ArgumentNullException(nameof(characterRanges));
            MeasurableCharacterRanges = characterRanges;
        }

        public object Clone()
        {
            StringFormat clonedObject = new StringFormat();
            clonedObject.FormatFlags = FormatFlags;
            clonedObject.MeasurableCharacterRanges = MeasurableCharacterRanges;
           
            return clonedObject; 
        }

        public CharacterRange[] MeasurableCharacterRanges { get; private set; }
    }
}