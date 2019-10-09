using System.Collections.Generic;

namespace RichTextBlock.Control
{
    public interface ITextRule
    {
        List<RuleText> ParserRule(ref string text);
    }
}