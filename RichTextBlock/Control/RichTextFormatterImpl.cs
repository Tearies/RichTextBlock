using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RichTextBlock.Control
{
    public class RichTextFormatterImpl
    {
        private string RichText { get; set; }
        private RichTextBlock host { get; set; }
        private Dictionary<RichTextRule, RichTextRuleParser> ParserDic { get; set; }
        public void SetUpRichTextHost(RichTextBlock block)
        {
            host = block;
            var rulesSource = host.Rules;
            RichText = host.Text;
            if (ParserDic == null)
                ParserDic = new Dictionary<RichTextRule, RichTextRuleParser>();
            else
                ParserDic.Clear();
            foreach (var richTextRule in rulesSource)
            {
                var parser = new RichTextRuleParser(richTextRule);
                ParserDic.Add(richTextRule, parser);
            }
        }

        public List<RuleText> Format()
        {
            var result = new List<RuleText>();
            var p = InternalFormat(RichText);
            if (p.Any())
            {
                p.ForEach(o=>result.AddRange(o.ToTexts()));
                
            }
            return result;
        }

        private List<RuleText> InternalFormat(string text)
        {
            var rulesText = new List<RuleText>();
            foreach (var richTextRule in ParserDic)
            {
                rulesText.AddRange(richTextRule.Value.ParserRule(ref text, InternalFormat));
            }
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
                        var trmp = host.BuildDefaultRule();
                        trmp.Offset = indexoffset;
                        trmp.Length = length;
                        trmp.Value = text.Substring(indexoffset, length);
                        missdRules.Add(trmp);
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
                var trmp = host.BuildDefaultRule();
                trmp.Offset = indexoffset;
                trmp.Length = text.Length;
                trmp.Value = text;
                tempRules.Add(trmp);
            }
            else
            {
                if (last.LengthWithMark < text.Length)
                {
                    var length = text.Length - last.LengthWithMark;
                    var trmp = host.BuildDefaultRule();
                    trmp.Offset = last.LengthWithMark;
                    trmp.Length = length;
                    trmp.Value = text.Substring(last.LengthWithMark, length);
                    tempRules.Add(trmp);
                }
            }
            tempRules.ForEach(o => o.Value = o.Value.Trim(RichTextRuleParser.SpaceChar));
            tempRules.RemoveAll(o => string.IsNullOrEmpty(o.Value));
            return tempRules;
        }
    }
}