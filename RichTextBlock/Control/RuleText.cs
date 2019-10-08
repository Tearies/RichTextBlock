using System.Windows;
using System.Windows.Media;

namespace RichTextBlock.Control
{
    public class RuleText
    {
        public int OffsetWithMark { get; set; }

        public int LengthWithMark { get; set; }

        public int Offset { get; set; }

        public string Value { get; set; }

        public int Length { get; set; }

        private double _fontSize;

        public double FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                FontSizeSet = true;
            }
        }

        public Brush Foreground { get; set; }

        public Brush Background { get; set; }

        public FontFamily FontFamily { get; set; }

        private FontStretch _fontStretch;

        public FontStretch FontStretch
        {
            get => _fontStretch;
            set
            {
                _fontStretch = value;
                FontStretchSet = true;
            }
        }

        private FontStyle _fontStyle;

        public FontStyle FontStyle
        {
            get => _fontStyle;
            set
            {
                _fontStyle = value;
                FontStyleSet = true;
            }
        }

        private FontWeight _fontWeight;

        public FontWeight FontWeight
        {
            get => _fontWeight;
            set
            {
                _fontWeight = value;
                FontWeightSet = true;
            }
        }

        public bool FontStyleSet { private set; get; }
        public bool FontWeightSet { private set; get; }
        public bool FontStretchSet { private set; get; }

        public bool FontSizeSet { private set; get; }
        
    }
}