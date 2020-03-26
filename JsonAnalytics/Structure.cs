using System;

namespace JsonAnalytics
{
    public enum StructuralChar
    {
        Whitespace,
        ArrayBegin,
        ArrayEnd,
        ObjectBegin,
        ObjectEnd,
        KeyValueSeparator,
        Comma,
        LeadingNegative,
        LeadingIntegerDigit,
        FollowingIntegerDigit,
        DecimalSeparator,
        ScientificNotationSeparator,
        StringDelimiter,
        UnescapedStringBody,
        StringEscapeMarker,
        SingleEscapedChar,
        UnicodeEscapedChar
    }
    
    public static class Structure
    {
        public static string AllRenderings(StructuralChar structuralChar)
        {
            return structuralChar switch
            {
                StructuralChar.Whitespace => " ",
                StructuralChar.ArrayBegin => "[",
                StructuralChar.ArrayEnd => "]",
                StructuralChar.Comma => ",",
                StructuralChar.LeadingNegative => "-",
                StructuralChar.LeadingIntegerDigit => "123456789",
                StructuralChar.FollowingIntegerDigit => "0123456789",
                StructuralChar.DecimalSeparator => ".",
                StructuralChar.ScientificNotationSeparator => "eE",
                StructuralChar.ObjectBegin => "{",
                StructuralChar.ObjectEnd => "}",
                StructuralChar.KeyValueSeparator => ":",
                StructuralChar.StringDelimiter => "\"",
                StructuralChar.UnescapedStringBody => " !#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~",
                StructuralChar.StringEscapeMarker => "\\",
                StructuralChar.SingleEscapedChar => "\"\\/bfnrt",
                StructuralChar.UnicodeEscapedChar => "0123456789abcdef", // TODO uppercase???
                _ => throw new ArgumentException("Unknown structural char")
            };
        }
    }
}