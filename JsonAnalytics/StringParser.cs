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
            EscapingUnicodeOne,
            EscapingUnicodeTwo,
            EscapingUnicodeThree,
            EscapingUnicodeFour,
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
                    NextChar(StructuralChar.UnicodeEscapeMarker, () => new StringParser(StringState.EscapingUnicodeOne));
                    break;
                case StringState.Completed:
                    break;
                case StringState.EscapingUnicodeOne:
                    NextChar(StructuralChar.UnicodeEscapedChar, () => new StringParser(StringState.EscapingUnicodeTwo));
                    break;
                case StringState.EscapingUnicodeTwo:
                    NextChar(StructuralChar.UnicodeEscapedChar, () => new StringParser(StringState.EscapingUnicodeThree));
                    break;
                case StringState.EscapingUnicodeThree:
                    NextChar(StructuralChar.UnicodeEscapedChar, () => new StringParser(StringState.EscapingUnicodeFour));
                    break;
                case StringState.EscapingUnicodeFour:
                    NextChar(StructuralChar.UnicodeEscapedChar, () => new StringParser(StringState.ReadyForChar));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == StringState.Completed;
    }
}