using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new ValueParser();
            var p2 = p1.Read('[');
            var p3 = p2.Read('1');
            var p4 = p3.Read(']');
            //
            // var nexts = string.Join("", p4.AcceptableChars());
            // Console.Out.WriteLine(nexts);

            var s = "";
            
            JsonParser parser = new ValueParser();

            for (var i = 0; i < 100; i++)
            {
                // var nextChar = RandomElement(parser.AcceptableChars());
                var nextChars = parser.AcceptableChars().ToList();
                if (!nextChars.Any())
                {
                    break;
                }

                var next = RandomElement(nextChars);
                s += next;
                parser = parser.Read(next);
            }
            
            Console.Out.WriteLine(s);
        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }
    }

    public class ArrayParser : JsonParser
    {
        private readonly ArrayState _state;

        public enum ArrayState
        {
            ReadyForFirst,
            ReadyForNext,
            JustReadValue,
            Completed
        }
        
        public ArrayParser(ArrayState state)
        {
            _state = state;
            switch (state)
            {
                case ArrayState.ReadyForFirst:
                    NextChar(ValueParser.ValueStarts, c => ValueParser.ParserForValue(c).ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return)));
                    NextChar(']', _ => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    break;
                case ArrayState.ReadyForNext:
                    NextChar(ValueParser.ValueStarts, c => ValueParser.ParserForValue(c).ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return)));
                    break;
                case ArrayState.JustReadValue:
                    NextChar(',', _ => new ArrayParser(ArrayState.ReadyForNext).ReturningTo(Return));
                    NextChar(']', _ => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    break;
                case ArrayState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == ArrayState.Completed;

        protected override void AssignReturn(JsonParser nextParser)
        {
            // We'll manually handle it
        }
    }

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

    public class StringParser
    {
        
    }
}