using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
        UnicodeEscapedChar,
        UnicodeEscapeMarker,
        OnlyZero,
        NullOne,
        NullTwo,
        NullThree,
        NullFour,
        TrueOne,
        TrueTwo,
        TrueThree,
        TrueFour,
        FalseOne,
        FalseTwo,
        FalseThree,
        FalseFour,
        FalseFive
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
                StructuralChar.UnescapedStringBody => " !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~",
                StructuralChar.StringEscapeMarker => "\\",
                StructuralChar.SingleEscapedChar => "\"\\/bfnrt",
                StructuralChar.UnicodeEscapedChar => "0123456789abcdefABCDEF",
                StructuralChar.UnicodeEscapeMarker => "u",
                StructuralChar.OnlyZero => "0",
                StructuralChar.NullOne => "n",
                StructuralChar.NullTwo => "u",
                StructuralChar.NullThree => "l",
                StructuralChar.NullFour => "l",
                StructuralChar.TrueOne => "t",
                StructuralChar.TrueTwo => "r",
                StructuralChar.TrueThree => "u",
                StructuralChar.TrueFour => "e",
                StructuralChar.FalseOne => "f",
                StructuralChar.FalseTwo => "a",
                StructuralChar.FalseThree => "l",
                StructuralChar.FalseFour => "s",
                StructuralChar.FalseFive => "e",
                _ => throw new ArgumentException("Unknown structural char")
            };
        }

        public static BigInteger Combinations(StructuralChar c)
        {
            return AllRenderings(c).Length;
        }

        public static BigInteger Combinations(IEnumerable<StructuralChar> chars)
        {
            return chars.Aggregate(BigInteger.One, (combos, c) => BigInteger.Multiply(combos, Combinations(c)));
        }

        public static string StringRepr(IEnumerable<StructuralChar> chars)
        {
            return string.Join(" ", chars.Select(s => s.ToString()));
        }
    }
}