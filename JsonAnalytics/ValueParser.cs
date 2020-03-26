using System;

namespace JsonAnalytics
{
    public class ValueParser : JsonParser
    {
        public static JsonParser ParserForValue(char c)
        {
            if (c == ' ') return new ValueParser();
            if (c == '[') return new ArrayParser(ArrayParser.ArrayState.ReadyForFirst);
            if (c == 'n') return new NullParser(NullParser.NullState.ReadN);
            if (c == '"') return new StringParser(StringParser.StringState.ReadyForChar);
            if (c == '{') return new ObjectParser();
            if (c == '0') return new NumberParser(NumberParser.NumberState.IsZero);
            if (c == '-') return new NumberParser(NumberParser.NumberState.ReadyForFirstDigit);
            if ("123456789".Contains(c)) return NumberParser.GetNumberParser(c);
            throw new ArgumentOutOfRangeException("Ruh roh");
        }

        public const string ValueStarts = "0123456789-[{ n\"";

        public ValueParser()
        {
            NextChar(StructuralChar.Whitespace, _ => this);
            NextChar(StructuralChar.Zero, _ => new NumberParser(NumberParser.NumberState.IsZero));
            NextChar(StructuralChar.LeadingNegative, _ => new NumberParser(NumberParser.NumberState.ReadyForFirstDigit));
            NextChar(StructuralChar.LeadingIntegerDigit, NumberParser.GetNumberParser);
            NextChar(StructuralChar.ArrayBegin, _ => new ArrayParser(ArrayParser.ArrayState.ReadyForFirst));
            NextChar('n', _ => new NullParser(NullParser.NullState.ReadN));
            NextChar(StructuralChar.StringDelimiter, _ => new StringParser(StringParser.StringState.ReadyForChar));
            NextChar(StructuralChar.ObjectBegin, _ => new ObjectParser());
        }

        public override bool CanComplete => false;
    }
}