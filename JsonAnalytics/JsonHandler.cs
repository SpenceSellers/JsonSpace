using System;
using System.Collections.Generic;
using System.Linq;
using JsonAnalytics.Parsing;

namespace JsonAnalytics
{
    public class JsonHandler
    {
        /// <summary>
        /// Generates a random valid JSON string
        /// </summary>
        public string RandomWalk()
        {

            var s = "[";
            var parser = new RootParser().Read('[');
            for (var i = 0; i < 250; i++)
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

            return s;
        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }

        public bool IsValidJson(string json)
        {
            JsonParser parser = new RootParser();
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
}