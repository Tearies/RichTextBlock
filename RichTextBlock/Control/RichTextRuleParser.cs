using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RichTextBlock.Control
{
    public class RichTextRuleParser : ITextRule
    {
        private RichTextRule Rule;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public RichTextRuleParser(RichTextRule rule)
        {
            Rule = rule;
        }

        private string BackStart { get; set; }

        private string BackEnd { get; set; }

        #region Implementation of ITextRule

        private static readonly List<string> RegexInvalidateChar = new List<string>
            {@"\", "^", "$", "*", "+", "?", ".", "(", ")", "[", "]"};

        internal static readonly char SpaceChar = (char)0x00;
         
        public List<RuleText> ParserRule(ref string text)
        {
            var start = Rule.Start;
            var end = Rule.End;
            BackStart = Rule.Start;
            BackEnd = Rule.End;
            if (!string.IsNullOrEmpty(start))
                RegexInvalidateChar.ForEach(o => start = start.Replace(o, @"\" + o));
            if (!string.IsNullOrEmpty(end))
                RegexInvalidateChar.ForEach(o => end = end.Replace(o, @"\" + o));
            string match = "";
            if (Rule.Regex != null)
            {
                match = Rule.Regex;
            }
            else
            {
                match = $"(?<={start}).*?(?={end})";
            }


            var result = Regex.Matches(text, match);
            var r = new List<RuleText>();
            foreach (Match rc in result)
            {
                var rt = new RuleText();
                rt.OffsetWithMark = rc.Index - (BackStart == null ? 0 : BackStart.Length);
                rt.FontStyle = Rule.FontStyle;
                rt.FontSize = Rule.FontSize;
                rt.Foreground = Rule.Foreground;
                rt.Background = Rule.Background;
                rt.Value = rc.Value;
                var tempValue = rc.Value.TrimStart(SpaceChar);
                rt.Offset = rc.Index + rc.Value.Length - tempValue.Length;
                rt.Length = rc.Length;
                rt.LengthWithMark = rc.Index + rc.Length + (BackEnd == null ? 0 : BackEnd.Length);
                r.Add(rt);
                text = text.Substring(0, rt.OffsetWithMark) + "".PadLeft(rt.LengthWithMark - rt.OffsetWithMark, SpaceChar) + text.Substring(rt.LengthWithMark, text.Length - rt.LengthWithMark);
            }
            return r;
        }

        #endregion
    }
}