using System.Windows;
using System.Windows.Media;

namespace RichTextBlock.Control
{
    public class RichTextRule
    {
        
        public string Start { get; set; }

        public string End { get; set; }

        public string Regex { get;  set; }

        public double FontSize { get; set; }

        public Brush Foreground { get; set; }

        public Brush Background { get; set; }

        public FontFamily FontFamily { get; set; }

        public FontStretch FontStretch { get; set; }

        public FontStyle FontStyle { get; set; }

        public FontWeight FontWeight { get; set; }
    }
}