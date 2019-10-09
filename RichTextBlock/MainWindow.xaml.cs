using System.Windows;
using System.Windows.Media;
using RichTextBlock.Control;

namespace RichTextBlock
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RichTextBlock.Foreground = Brushes.Black;
            RichTextBlock.FontSize = 24;
            RichTextBlock.Text =
                "<aaa>12123<a>测试</a>我大师邦</aaa>23123<a>12123</a>121(234)5555[456]666{778}<<>ccccc</<>$222/$!2/!@3/@%5/%^6/^&99/&*aa/*+abc/+";
            
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Regex = @"\d",
                Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0xF0)),
                FontSize = 24
            });

            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "<aaa>",
                End = "</aaa>",
                Foreground = Brushes.Red,
                FontSize = 24
            });

            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "<a>",
                End = "</a>",
                Foreground = Brushes.Blue,
                FontSize = 24
            });

        

            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "<<>",
                End = "</<>",
                Foreground = Brushes.BlueViolet,
                FontSize = 24
            });

            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "{",
                End = "}",
                Foreground = Brushes.YellowGreen,
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "!",
                End = "/!",
                Foreground = Brushes.Pink,
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "@",
                End = "/@",
                Foreground = Brushes.Brown,
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "$",
                End = "/$",
                Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x0d, 0xcc, 0x01)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "%",
                End = "/%",
                Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x1a, 0xcc, 0x8a)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "^",
                End = "/^",
                Foreground = new SolidColorBrush(Color.FromArgb(0x77, 0x1a, 0xcc, 0x8a)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "&",
                End = "/&",
                Foreground = new SolidColorBrush(Color.FromArgb(0x77, 0x1a, 0xcc, 0x8a)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "+",
                End = "/+",
                Foreground = new SolidColorBrush(Color.FromArgb(0xD7, 0x1a, 0xFc, 0x8a)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "*",
                End = "/*",
                Foreground = new SolidColorBrush(Color.FromArgb(0xF7, 0xFa, 0xcc, 0x8F)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "(",
                End = ")",
                Foreground = new SolidColorBrush(Color.FromArgb(0xF7, 0xFa, 0xFc, 0x8F)),
                FontSize = 24
            });
            RichTextBlock.Rules.Add(new RichTextRule
            {
                Start = "[",
                End = "]",
                Foreground = new SolidColorBrush(Color.FromArgb(0xF7, 0xFa, 0xd0, 0xF0)),
                FontSize = 24
            });
          
        }
    }
}