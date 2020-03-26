using System;

namespace JsonAnalytics
{
    public class StringParser : JsonParser
    {
        private readonly StringState _state;

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
                    NextChar(StructuralChar.StringEscapeMarker, () => new StringParser(StringState.Escaping));
                    NextChar(StructuralChar.UnescapedStringBody, () => new StringParser(StringState.ReadyForChar));
                    NextChar(StructuralChar.StringDelimiter, () => new StringParser(StringState.Completed));
                    break;
                case StringState.Escaping:
                    NextChar(StructuralChar.SingleEscapedChar, () => new StringParser(StringState.ReadyForChar));
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