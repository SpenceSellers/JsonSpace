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

    public class JsonHandler
    {
        public bool IsValidJson(string json)
        {
            JsonParser parser = new ValueParser();
            foreach (var c in json)
            {
                if (!parser.AcceptableChars().Contains(c))
                {
                    return false;
                }
                
                parser = parser.Read(c);
            }

            return parser.CanBeTheEndOfInput;
        }
    }

    public class NumberParser : JsonParser
    {
        private readonly NumberState _state;

        public static NumberParser GetNumberParser(char c)
        {
            if (c == '0')
            {
                return new NumberParser(NumberState.IsZero);
            }
            
            if (c == '-')
            {
                return new NumberParser(NumberState.ReadyForFirstDigit);
            }

            if ("123456789".Contains(c))
            {
                return new NumberParser(NumberState.SecondaryDigit);
            }
            
            throw new ArgumentException("Bad Number start: " + c);
        }
        
        public enum NumberState
        {
            IsZero,
            ReadyForFirstDigit,
            SecondaryDigit,
            ReadyForFraction,
            ReadyToContinueFraction,
            ReadyForExponentStart,
            ReadyToContinueExponent
        }
        public NumberParser(NumberState state)
        {
            _state = state;
            switch (state)
            {
                case NumberState.IsZero:
                    break;
                case NumberState.ReadyForFirstDigit:
                    NextChar("123456789", _ => new NumberParser(NumberState.SecondaryDigit));
                    break;
                case NumberState.SecondaryDigit:
                    NextChar("0123456789", _ => new NumberParser(NumberState.SecondaryDigit));
                    NextChar(".", _ => new NumberParser(NumberState.ReadyForFraction));
                    NextChar("eE", _ => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForFraction:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueFraction));
                    break;
                case NumberState.ReadyToContinueFraction:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueFraction));
                    NextChar("eE", _ => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForExponentStart:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueExponent));
                    break;
                case NumberState.ReadyToContinueExponent:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueExponent));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state switch
        {
            NumberState.IsZero => true,
            NumberState.ReadyForFirstDigit => false,
            NumberState.SecondaryDigit => true,
            NumberState.ReadyForFraction => false,
            NumberState.ReadyToContinueFraction => true,
            NumberState.ReadyForExponentStart => false,
            NumberState.ReadyToContinueExponent => true,
        };
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