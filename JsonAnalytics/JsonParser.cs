using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    public abstract class JsonParser
    {
        private readonly Dictionary<char, Func<JsonParser>> _parsers = new Dictionary<char, Func<JsonParser>>();
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
                var parser =  _parsers[c]();
                AssignReturn(parser);
                return parser;
            }

            if (CanComplete && Return != null)
            {
                return Return.Read(c);
            }

            throw new ArgumentException("Cannot read " + c);
        }

        protected virtual void AssignReturn(JsonParser nextParser)
        {
            nextParser.ReturningTo(Return);
        }

        public abstract bool CanComplete { get; }

        public bool CanBeTheEndOfInput => Return == null && CanComplete;

        public JsonParser ReturningTo(JsonParser parent)
        {
            Return = parent;
            return this;
        }
        
        protected void NextChar(char c, Func<JsonParser> parser)
        {
            _parsers[c] = parser;
        }
        
        protected void NextChar(StructuralChar schar, Func<JsonParser> parser)
        {
            var renderings = Structure.AllRenderings(schar);
            NextChar(renderings, parser);
        }

        protected void NextChar(IEnumerable<char> chars, Func<JsonParser> parser)
        {
            foreach (var c in chars)
            {
                NextChar(c, parser);
            }
        }

        protected void NextCanBeValueReturningTo(Func<JsonParser> returnParser)
        {
            // Gross. The returning parser has to be a thunk because the return value will probably be set on the caller AFTER the constructor is run.
            // Definitely need to rethink something here, like use real builders instead of constructors.
            
            NextChar(StructuralChar.Whitespace, () => this);
            NextChar(StructuralChar.OnlyZero, () => new NumberParser(NumberParser.NumberState.IsZero).ReturningTo(returnParser()));
            NextChar(StructuralChar.LeadingNegative, () => new NumberParser(NumberParser.NumberState.ReadyForFirstDigit).ReturningTo(returnParser()));
            NextChar(StructuralChar.LeadingIntegerDigit, () => new NumberParser(NumberParser.NumberState.SecondaryDigit).ReturningTo(returnParser()));
            NextChar(StructuralChar.ArrayBegin, () => new ArrayParser(ArrayParser.ArrayState.ReadyForFirst).ReturningTo(returnParser()));
            NextChar(StructuralChar.NullOne, () => new NullParser(NullParser.NullState.ReadN).ReturningTo(returnParser()));
            NextChar(StructuralChar.StringDelimiter, () => new StringParser(StringParser.StringState.ReadyForChar).ReturningTo(returnParser()));
            NextChar(StructuralChar.ObjectBegin, () => new ObjectParser().ReturningTo(returnParser()));
        }
    }
}