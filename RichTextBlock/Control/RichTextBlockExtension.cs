using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace RichTextBlock.Control
{
    public static class RichTextBlockExtension
    {
        public static double[] GetCharWidths(this FormattedText formattedText, RichTextBlock richTextBlock,RuleText rule )
        {
            richTextBlock.ValidateDefaultRule(rule);
            var text = formattedText.Text;
            var result = new double[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                var ft = new FormattedText(text[i].ToString(), CultureInfo.CurrentUICulture, richTextBlock.FlowDirection,  new Typeface(rule.FontFamily,rule.FontStyle,rule.FontWeight,rule.FontStretch), rule.FontSize, rule.Foreground);
                result[i] = ft.Width;
            }
            return result;
        }

        public static RuleText BuildDefaultRule(this RichTextBlock richTextBlock)
        {
            return richTextBlock.ValidateDefaultRule(null);
        }

        public static RuleText ValidateDefaultRule(this RichTextBlock richTextBlock, RuleText currentRule)
        {
            if (currentRule == null)
            {
                currentRule=new RuleText();
                currentRule.IsUnMatched = true;
            }
            if (!currentRule.FontStyleSet)
                currentRule.FontStyle = richTextBlock.FontStyle;
            if(!currentRule.FontWeightSet)
                currentRule.FontWeight = richTextBlock.FontWeight;
            if (currentRule.FontFamily == null)
                currentRule.FontFamily = richTextBlock.FontFamily;
            if(!currentRule.FontStretchSet)
                currentRule.FontStretch = richTextBlock.FontStretch;
            if (currentRule.Background == null)
                currentRule.Background = richTextBlock.Background;
            if (currentRule.Foreground == null)
                currentRule.Foreground = richTextBlock.Foreground;
            if(!currentRule.FontSizeSet)
                currentRule.FontSize = richTextBlock.FontSize;
            return currentRule;
        }

        public static FormattedText BuildFormattedText(this RichTextBlock richTextBlock,string value, RuleText currentRule=null)
        {
            if (currentRule == null)
                currentRule = richTextBlock.BuildDefaultRule();
            else
            { 
                richTextBlock.ValidateDefaultRule(currentRule);
            }
            return new FormattedText(value, CultureInfo.InvariantCulture,
                richTextBlock.FlowDirection,
                new Typeface(currentRule.FontFamily, currentRule.FontStyle,
                    currentRule.FontWeight, currentRule.FontStretch),
                currentRule.FontSize, currentRule.Foreground);
        }
    }
}