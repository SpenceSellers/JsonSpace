using System;

namespace JsonAnalytics
{
    public class ValueParser : JsonParser
    {
        public static JsonParser ParserForValue(char c)
        {
            if (c == '[') return new ArrayParser(ArrayParser.ArrayState.ReadyForFirst);
            if ("-0123456789".Contains(c)) return NumberParser.GetNumberParser(c);
            throw new ArgumentOutOfRangeException("Ruh roh");
        }

        public const string ValueStarts = "0123456789-[{";

        public ValueParser()
        {
            NextChar("0123456789-", NumberParser.GetNumberParser);
            NextChar('[', _ => new ArrayParser(ArrayParser.ArrayState.ReadyForFirst));
        }

        public override bool CanComplete => false;
    }
}