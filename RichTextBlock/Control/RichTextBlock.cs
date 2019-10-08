using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PresentationFramework;

namespace RichTextBlock.Control
{
    public class RichTextBlock : FrameworkElement
    {
        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(RichTextBlock),
                new FrameworkPropertyMetadata(string.Empty, OnTextPropertyChangedCallBack));
        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(RichTextBlock));
        public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(RichTextBlock), (PropertyMetadata)new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.AffectsRender));

        public FontFamily FontFamily
        {
            get
            {
                return (FontFamily)this.GetValue(FontFamilyProperty);
            }
            set
            {
                this.SetValue(FontFamilyProperty, (object)value);
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return (FontStyle)this.GetValue(FontStyleProperty);
            }
            set
            {
                this.SetValue(FontStyleProperty, (object)value);
            }
        }
        public FontWeight FontWeight
        {
            get
            {
                return (FontWeight)this.GetValue(FontWeightProperty);
            }
            set
            {
                this.SetValue(FontWeightProperty, (object)value);
            }
        }

        public FontStretch FontStretch
        {
            get
            {
                return (FontStretch)this.GetValue(FontStretchProperty);
            }
            set
            {
                this.SetValue(FontStretchProperty, (object)value);
            }
        }

        public double FontSize
        {
            get
            {
                return (double)this.GetValue(FontSizeProperty);
            }
            set
            {
                this.SetValue(FontSizeProperty, (object)value);
            }
        }

        public Brush Foreground
        {
            get
            {
                return (Brush)this.GetValue(ForegroundProperty);
            }
            set
            {
                this.SetValue(ForegroundProperty, (object)value);
            }
        }

        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(BackgroundProperty);
            }
            set
            {
                this.SetValue(BackgroundProperty, (object)value);
            }
        }
        private static void OnTextPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RichTextBlock)d).OnTextPropertyChangedCallBack();
        }

        private void OnTextPropertyChangedCallBack()
        {
            formatter?.FormatBlock(this);
        }

        public static readonly DependencyProperty AutoWrapProperty =
            DependencyProperty.Register(nameof(AutoWrap), typeof(bool), typeof(RichTextBlock), new FrameworkPropertyMetadata(true, OnAutoWrapPropertyChangedCallBack));

        private static void OnAutoWrapPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RichTextBlock)d).OnAutoWrapPropertyChangedCallBack();
        }

        private void OnAutoWrapPropertyChangedCallBack()
        {
            formatter?.FormatBlock(this);
        }


        private readonly RichTextFormatter formatter;

        private FormatFrameArgs prevFrame;

        public RichTextBlock()
        {
            Rules = new ObservableCollection<RichTextRule>();
            Rules.CollectionChanged += Rules_CollectionChanged;
            formatter = new RichTextFormatter();
            formatter.FrameRecived += Formatter_FrameRecived;
        }

        public ObservableCollection<RichTextRule> Rules { get; internal set; }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool AutoWrap
        {
            get { return (bool)GetValue(AutoWrapProperty); }
            set { SetValue(AutoWrapProperty, value); }
        }

        private void Formatter_FrameRecived(object sender, FormatFrameArgs e)
        {
            if (prevFrame != null)
            {
                prevFrame.Dispose();
                prevFrame = null;
            }
            prevFrame = e;
            InvalidateVisual();
        }

        private void Rules_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            formatter.FormatBlock(this);
        }

        #region Overrides of FrameworkElement
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            formatter.FormatBlock(this);
        }

        #endregion

        #region Overrides of UIElement
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (prevFrame == null)
                return;
            var frame = prevFrame.MemClone();
            if (frame.TextFrames.Any())
                foreach (var tempRule in frame.TextFrames)
                    drawingContext.DrawText(tempRule.FormattedText, tempRule.Location);
            frame.Dispose();
            frame = null;
        }

        #endregion

        #region Overrides of FrameworkElement

        protected override Size MeasureOverride(Size availableSize)
        {
            var resultSize = base.MeasureOverride(availableSize);
            formatter.FormatBlock(this);
            return resultSize;
        }

        #endregion
    }
}