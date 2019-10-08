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
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public RichTextFormatter()
        {
            FormatCache = new ObservableCollection<TextFormatCache>();
            FormatCache.CollectionChanged += FormatCache_CollectionChanged;
        }

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
                BuildFormatText(richTextBlock, ParseTextRule(richTextBlock, richTextBlock.Rules, text));
            }
            else
            {
                var formatText = new FormattedText(richTextBlock.Text, CultureInfo.CurrentUICulture,
                    richTextBlock.FlowDirection,
                      new Typeface(richTextBlock.FontFamily, richTextBlock.FontStyle,
                          richTextBlock.FontWeight, richTextBlock.FontStretch),
                    richTextBlock.FontSize, Brushes.Black);
                FormatCache.Add(new TextFormatCache(formatText, new Point()));
            }
        }

        private List<RuleText> ParseTextRule(RichTextBlock richTextBlock,IList<RichTextRule> rules, string text)
        {
            var rulesText = new List<RuleText>();
            foreach (var richTextRule in rules)
                rulesText.AddRange(new RichTextRuleParser(richTextRule).ParserRule(text));
            //匹配出来的字符串
            var tempRules = rulesText.OrderBy(o => o.Offset).ToList();
            //查找配有匹配出来的字符串
            var indexoffset = 0;
            var missdRules = new List<RuleText>();
            foreach (var tmrule in tempRules)
            {
                if (tmrule.OffsetWithMark > indexoffset)
                    try
                    {
                        var length = tmrule.OffsetWithMark - indexoffset;
                        var rule = richTextBlock.BuildDefaultRule();
                        rule.Offset = indexoffset;
                        rule.Length = length;
                        rule.Value = text.Substring(indexoffset, length);
                        missdRules.Add(rule);
                    }
                    catch (Exception e)
                    {
                    }

                indexoffset = tmrule.LengthWithMark;
            }

            tempRules.AddRange(missdRules);
            tempRules = tempRules.OrderBy(o => o.Offset).ToList();
            var last = tempRules.LastOrDefault();
            if (last == null)
            {
                var rule = richTextBlock.BuildDefaultRule();
                rule.Offset = indexoffset;
                rule.Length = text.Length;
                rule.Value = text;
                tempRules.Add(rule);
            }
            else
            {
                if (last.LengthWithMark < text.Length)
                {
                    var length = text.Length - last.LengthWithMark;
                    var rule = richTextBlock.BuildDefaultRule();
                    rule.Offset = last.LengthWithMark;
                    rule.Length = length;
                    rule.Value = text.Substring(last.LengthWithMark, length);
                    tempRules.Add( rule);
                }
            }
            return tempRules;
        }

        private void BuildFormatText(RichTextBlock richTextBlock, List<RuleText> tempRules)
        {
            var offset = new Point(0, 0);
            foreach (var tempRule in tempRules)
            {
                var formatText = richTextBlock.BuildFormattedText(tempRule.Value,tempRule);

                var ftext = formatText.Text;
                if (offset.X + formatText.Width > richTextBlock.ActualWidth)
                {
                    if (richTextBlock.AutoWrap)
                    {
                        while (offset.X + formatText.Width > richTextBlock.ActualWidth)
                        {
                            var eachWidths = formatText.GetCharWidths(richTextBlock,tempRule).ToArray();
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
                            formatText = richTextBlock.BuildFormattedText(ftext,tempRule);
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