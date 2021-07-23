using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics.Parsing
{
    public abstract class JsonParser
    {
        private readonly Dictionary<StructuralChar, Func<JsonParser>> _parsers = new Dictionary<StructuralChar, Func<JsonParser>>();
        protected JsonParser Return;

        /// <summary>
        /// Provides all possible characters that will continue this string without creating an invalid JSON string.
        /// </summary>
        public IEnumerable<char> AcceptableChars()
        {
            return string.Join("", AcceptableStructuralChars().Select(Structure.AllRenderings));
        }

        /// <summary>
        /// Provides all possible structural characters that will continue this string without creating an invalid JSON string.
        /// </summary>
        public IEnumerable<StructuralChar> AcceptableStructuralChars()
        {
            var structuralChars =  _parsers.Keys.ToList();
            if (CanComplete && Return != null)
            {
                // Since this parser can complete at this very moment, all of the acceptable characters
                // for the NEXT parser are also valid here.
                structuralChars.AddRange(Return.AcceptableStructuralChars());
            }

            return structuralChars;
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

            throw new ArgumentException($"{GetType().Name} Cannot read " + c);
        }

        public JsonParser ReadAll(IEnumerable<StructuralChar> chars)
        {
            var parser = this;
            foreach (var structuralChar in chars)
            {
                parser = parser.Read(structuralChar);
            }

            return parser;
        }

        /// <summary>
        /// Determine what structural character a char represents and then read it
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

        /// <summary>
        /// If the string we're parsing stopped at this point, would it be a valid JSON string?
        ///
        /// Note that just because a string is a valid JSON string, doesn't mean that adding more characters will break
        /// the string. For example, adding more characters to the end of "[1,2]" will break the string, but adding more
        /// numbers to the end of "1.348" will continue to produce valid JSON strings forever.
        /// </summary>
        public bool CanBeTheEndOfInput => (Return == null || Return.IsNotNeededToComplete) && CanComplete;

        /// <summary>
        /// Defines what parser will take over once this parser is done.
        /// </summary>
        public JsonParser ReturningTo(JsonParser? parent)
        {
            Return = parent;
            return this;
        }

        /// <summary>
        /// Defines what parser will take over when we read a StructuralChar
        /// </summary>
        protected void NextChar(StructuralChar schar, Func<JsonParser> parser)
        {
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
            NextChar(StructuralChar.FalseOne, () => new FalseParser(FalseParser.FalseState.ReadF).ReturningTo(returnParser()));
            NextChar(StructuralChar.StringDelimiter, () => new StringParser(StringParser.StringState.ReadyForChar).ReturningTo(returnParser()));
            NextChar(StructuralChar.ObjectBegin, () => new ObjectParser().ReturningTo(returnParser()));
        }
    }
}