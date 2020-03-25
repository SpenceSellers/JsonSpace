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

    public abstract class JsonParser
    {
        private readonly Dictionary<char, Func<char, JsonParser>> _parsers = new Dictionary<char, Func<char, JsonParser>>();
        protected JsonParser Return;

        public IEnumerable<char> AcceptableChars()
        {
            var aa =  _parsers.Keys.ToList();
            if (CanComplete && Return != null)
            {
                aa.AddRange(Return.AcceptableChars());
            }

            return aa;
        }

        public JsonParser Read(char c)
        {
            if (_parsers.ContainsKey(c))
            {
                return _parsers[c](c);
            }

            if (CanComplete && Return != null)
            {
                return Return.Read(c);
            }

            throw new ArgumentException("Cannot read " + c);
        }

        public abstract bool CanComplete { get; }

        public bool CanBeTheEndOfInput => Return == null && CanComplete;

        public JsonParser ReturningTo(JsonParser parent)
        {
            Return = parent;
            return this;
        }
        
        protected void NextChar(char c, Func<char, JsonParser> parser)
        {
            _parsers.Add(c, parser);
        }

        protected void NextChar(IEnumerable<char> chars, Func<char, JsonParser> parser)
        {
            foreach (var c in chars)
            {
                NextChar(c, parser);
            }
        }
    }

    public class NumberParser : JsonParser
    {
        public NumberParser()
        {
            NextChar("0123456789", _ => new NumberParser());
        }

        public override bool CanComplete => true;
    }

    public class ArrayParser : JsonParser
    {
        private readonly ArrayState _state;

        public enum ArrayState
        {
            ReadyForNext,
            JustReadValue,
            Completed
        }
        
        public ArrayParser(ArrayState state)
        {
            _state = state;
            switch (state)
            {
                case ArrayState.ReadyForNext:
                    NextChar(ValueParser.ValueStarts, c => ValueParser.ParserForValue(c).ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return)));
                    break;
                case ArrayState.JustReadValue:
                    NextChar(']', _ => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    NextChar(',', _ => new ArrayParser(ArrayState.ReadyForNext).ReturningTo(Return));
                    break;
                case ArrayState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == ArrayState.Completed;
    }

    public class ValueParser : JsonParser
    {
        public static JsonParser ParserForValue(char c)
        {
            if (c == '[') return new ArrayParser(ArrayParser.ArrayState.ReadyForNext);
            if ("0123456789".Contains(c)) return new NumberParser();
            throw new ArgumentOutOfRangeException("Ruh roh");
        }

        public static readonly string ValueStarts = "0123456789[{";

        public ValueParser()
        {
            NextChar("0123456789", _ => new NumberParser());
            NextChar('[', _ => new ArrayParser(ArrayParser.ArrayState.ReadyForNext));
        }

        public override bool CanComplete => false;
    }

    public class StringParser
    {
        
    }
}