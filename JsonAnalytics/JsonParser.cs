using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    public abstract class JsonParser
    {
        private readonly Dictionary<StructuralChar, Func<JsonParser>> _parsers = new Dictionary<StructuralChar, Func<JsonParser>>();
        protected JsonParser Return;

        public IEnumerable<char> AcceptableChars()
        {
            return string.Join("", AcceptableStructuralChars().Select(Structure.AllRenderings));
        }
        
        public IEnumerable<StructuralChar> AcceptableStructuralChars()
        {
            var aa =  _parsers.Keys.ToList();
            if (CanComplete && Return != null)
            {
                aa.AddRange(Return.AcceptableStructuralChars());
            }

            return aa;
        }

        public JsonParser Read(StructuralChar c)
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

        /// <summary>
        /// Make a best guess at what structural character a char represents and then read it
        /// </summary>
        public JsonParser Read(char c)
        {
            foreach (var structuralChar in AcceptableStructuralChars())
            {
                if (Structure.AllRenderings(structuralChar).Contains(c))
                {
                    return Read(structuralChar);
                }
            }
            
            throw new ArgumentException("Cannot read " + c);
        }

        protected virtual void AssignReturn(JsonParser nextParser)
        {
            nextParser.ReturningTo(Return);
        }

        public abstract bool CanComplete { get; }
        
        // If true, then it's OK if the input ends before this has been satisfied.
        protected virtual bool IsNotNeededToComplete => false;

        public bool CanBeTheEndOfInput => (Return == null || Return.IsNotNeededToComplete) && CanComplete;

        public JsonParser ReturningTo(JsonParser parent)
        {
            Return = parent;
            return this;
        }

        protected void NextChar(StructuralChar schar, Func<JsonParser> parser)
        {
            // var renderings = Structure.AllRenderings(schar);
            // NextChar(renderings, parser);
            _parsers[schar] = parser;
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
            NextChar(StructuralChar.TrueOne, () => new TrueParser(TrueParser.TrueState.ReadT).ReturningTo(returnParser()));
            NextChar(StructuralChar.StringDelimiter, () => new StringParser(StringParser.StringState.ReadyForChar).ReturningTo(returnParser()));
            NextChar(StructuralChar.ObjectBegin, () => new ObjectParser().ReturningTo(returnParser()));
        }
    }
}