using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace JsonAnalytics
{
    public static class Structure
    {
        /// <summary>
        /// Returns all possible values that a StructuralChar can represent.
        /// </summary>
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

        /// <summary>
        /// How many strings could this single StructuralChar represent?
        /// </summary>
        private static BigInteger Combinations(StructuralChar c)
        {
            return AllRenderings(c).Length;
        }

        /// <summary>
        /// How many strings could this sequence of StructuralChars represent?
        /// </summary>
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