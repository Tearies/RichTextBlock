using System.Windows;
using System.Windows.Media;

namespace RichTextBlock.Control
{
    public class TextFormatCache
    {
        public FormattedText FormattedText;
        public Point Location;

        public TextFormatCache(FormattedText formatText, Point offset)
        {
            FormattedText = formatText;
            Location = offset;
        }
    }
}