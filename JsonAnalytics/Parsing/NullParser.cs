using System;

namespace JsonAnalytics.Parsing
{
    public class NullParser : JsonParser
    {
        private readonly NullState _state;

        public enum NullState
        {
            ReadN,
            ReadU,
            ReadFirstL,
            ReadSecondL
        }

        public NullParser(NullState state)
        {
            _state = state;
            switch (state)
            {
                case NullState.ReadN:
                    NextChar(StructuralChar.NullTwo, () => new NullParser(NullState.ReadU));
                    break;
                case NullState.ReadU:
                    NextChar(StructuralChar.NullThree, () => new NullParser(NullState.ReadFirstL));
                    break;
                case NullState.ReadFirstL:
                    NextChar(StructuralChar.NullFour, () => new NullParser(NullState.ReadSecondL));
                    break;
                case NullState.ReadSecondL:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == NullState.ReadSecondL;
    }
}