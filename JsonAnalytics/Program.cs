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

            for (var n = 0; n < 10; n++)
            {
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

        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }
    }

    public class StringParser : JsonParser
    {
        private readonly StringState _state;

        // All characters that can appear in the middle of a string, unescaped. Minus backslash, which is USED to escape.
        private const string UnescapedChars =
            " !#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        
        // All singular characters that can appear in escaped form. This doesn't include unicode.
        private const string SimpleEscapes = "\"\\/bfnrt";

        public enum StringState
        {
            ReadyForChar,
            Escaping,
            // TODO escaping unicode
            Completed
        }

        public StringParser(StringState state)
        {
            _state = state;
            switch (state)
            {
                case StringState.ReadyForChar:
                    NextChar('\\', _ => new StringParser(StringState.Escaping));
                    NextChar(UnescapedChars, _ => new StringParser(StringState.ReadyForChar));
                    NextChar("\"", _ => new StringParser(StringState.Completed));
                    break;
                case StringState.Escaping:
                    NextChar(SimpleEscapes, _ => new StringParser(StringState.ReadyForChar));
                    break;
                case StringState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == StringState.Completed;
    }
}