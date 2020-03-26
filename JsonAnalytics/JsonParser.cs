using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
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
                var parser =  _parsers[c](c);
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
        
        protected void NextChar(char c, Func<char, JsonParser> parser)
        {
            _parsers[c] = parser;
        }

        protected void NextChar(IEnumerable<char> chars, Func<char, JsonParser> parser)
        {
            foreach (var c in chars)
            {
                NextChar(c, parser);
            }
        }
    }
}