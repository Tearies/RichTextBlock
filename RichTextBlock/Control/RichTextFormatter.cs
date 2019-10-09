using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Media;

namespace RichTextBlock.Control
{
    public class RichTextFormatter
    {
        public RichTextFormatter()
        {
            FormatterImpl = new RichTextFormatterImpl();
            FormatCache = new ObservableCollection<TextFormatCache>();
            FormatCache.CollectionChanged += FormatCache_CollectionChanged;
        }

        private RichTextFormatterImpl FormatterImpl { get; }
        public ObservableCollection<TextFormatCache> FormatCache { get; }

        private void FormatCache_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnFrameRecived();
        }

        internal event EventHandler<FormatFrameArgs> FrameRecived;

        public void FormatBlock(RichTextBlock richTextBlock)
        {
            var text = richTextBlock.Text;
            if (string.IsNullOrEmpty(text))
                return;

            if (richTextBlock.Rules.Any())
            {
                FormatCache.Clear();
                FormatterImpl.SetUpRichTextHost(richTextBlock);
                BuildFormatText(richTextBlock, FormatterImpl.Format());
            }
            else
            {
                var formatText = richTextBlock.BuildFormattedText(text);
                FormatCache.Add(new TextFormatCache(formatText, new Point()));
            }
        }

        private void BuildFormatText(RichTextBlock richTextBlock, List<RuleText> tempRules)
        {
            var offset = new Point(0, 0);
            foreach (var tempRule in tempRules)
            {
                var formatText = richTextBlock.BuildFormattedText(tempRule.Value, tempRule);
                var ftext = formatText.Text;
                if (offset.X + formatText.Width > richTextBlock.ActualWidth)
                {
                    if (richTextBlock.AutoWrap)
                    {
                        while (offset.X + formatText.Width > richTextBlock.ActualWidth)
                        {
                            var eachWidths = formatText.GetCharWidths(richTextBlock, tempRule).ToArray();
                            double width = 0;
                            var needRemoveCount = eachWidths.Length;
                            do
                            {
                                needRemoveCount--;
                                width = eachWidths.Take(needRemoveCount).Sum();
                                if (needRemoveCount < 0)
                                    break;
                            } while (offset.X + width > richTextBlock.ActualWidth);

                            var renderText = ftext.Substring(0, needRemoveCount);
                            formatText = richTextBlock.BuildFormattedText(renderText, tempRule);
                            FormatCache.Add(new TextFormatCache(formatText, offset));
                            offset.Offset(formatText.Width, 0);
                            formatText = null;
                            ftext = ftext.Substring(needRemoveCount, ftext.Length - needRemoveCount);
                            if (string.IsNullOrEmpty(ftext))
                                break;
                            formatText = richTextBlock.BuildFormattedText(ftext, tempRule);
                            offset.Offset(-offset.X, formatText.Height);
                        }

                        if (formatText != null)
                        {
                            FormatCache.Add(new TextFormatCache(formatText, offset));
                            offset.Offset(formatText.Width, 0);
                        }
                    }
                    else
                    {
                        offset.Offset(-offset.X, formatText.Height);
                        FormatCache.Add(new TextFormatCache(formatText, offset));
                        offset.Offset(formatText.Width, 0);
                    }
                }
                else
                {
                    FormatCache.Add(new TextFormatCache(formatText, offset));
                    offset.Offset(formatText.Width, 0);
                }
            }
        }

        protected virtual void OnFrameRecived()
        {
            FrameRecived?.Invoke(this, new FormatFrameArgs(FormatCache));
        }
    }
}