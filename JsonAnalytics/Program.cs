using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            ThingParser parser = new ValueParser();
            parser = parser.Read('[');
            parser = parser.Read('1');
            var nexts = string.Join("", parser.AcceptableChars());
            Console.Out.WriteLine(nexts);
        }
    }

    public class Parser
    {
        
    }

    public abstract class ThingParser
    {
        private readonly Dictionary<char, Func<ThingParser>> _parsers = new Dictionary<char, Func<ThingParser>>();
        protected ThingParser _return;

        public IEnumerable<char> AcceptableChars()
        {
            var aa =  _parsers.Keys.ToList();
            if (CanComplete() && _return != null)
            {
                aa.AddRange(_return.AcceptableChars());
            }

            return aa;
        }

        public ThingParser Read(char c)
        {
            return _parsers[c]();
        }

        public abstract bool CanComplete();

        public ThingParser ReturningTo(ThingParser parent)
        {
            _return = parent;
            return this;
        }
        
        protected void NextChar(char c, Func<ThingParser> parser)
        {
            _parsers.Add(c, parser);
        }

        protected void NextChar(IEnumerable<char> chars, Func<ThingParser> parser)
        {
            foreach (var c in chars)
            {
                NextChar(c, parser);
            }
        }
    }

    public class NumberParser : ThingParser
    {
        public NumberParser()
        {
            NextChar("0123456789", () => new NumberParser());
        }

        public override bool CanComplete()
        {
            return true;
        }
    }

    public class ArrayParser : ThingParser
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
                    NextChar(ValueParser.ValueStarts, () => new ValueParser().ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(_return)));
                    break;
                case ArrayState.JustReadValue:
                    // TODO comma
                    NextChar(']', () => new ArrayParser(ArrayState.Completed).ReturningTo(_return));
                    break;
                case ArrayState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete()
        {
            // TODO this isn't true
            return true;
        }
    }

    public class ValueParser : ThingParser
    {
        public static readonly string ValueStarts = "0123456789[{";
        public ValueParser()
        {
            NextChar("0123456789", () => new NumberParser());
            NextChar('[', () => new ArrayParser(ArrayParser.ArrayState.ReadyForNext));
        }

        public override bool CanComplete()
        {
            return false;
        }
    }

    public class StringParser
    {
        
    }
}