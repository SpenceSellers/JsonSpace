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
            if ("-0123456789".Contains(c)) return NumberParser.GetNumberParser(c);
            throw new ArgumentOutOfRangeException("Ruh roh");
        }

        public const string ValueStarts = "0123456789-[{ n\"";

        public ValueParser()
        {
            NextChar(' ', ParserForValue);
            NextChar("0123456789-", NumberParser.GetNumberParser);
            NextChar('[', _ => new ArrayParser(ArrayParser.ArrayState.ReadyForFirst));
            NextChar('n', _ => new NullParser(NullParser.NullState.ReadN));
            NextChar('"', _ => new StringParser(StringParser.StringState.ReadyForChar));
            NextChar('{', _ => new ObjectParser());
        }

        public override bool CanComplete => false;
    }
}