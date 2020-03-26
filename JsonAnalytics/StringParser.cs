using System;

namespace JsonAnalytics
{
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

        public StringParser() : this(StringState.ReadyForChar)
        {
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