using System;

namespace JsonAnalytics
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
                    NextChar('u', _ => new NullParser(NullState.ReadU));
                    break;
                case NullState.ReadU:
                    NextChar('l', _ => new NullParser(NullState.ReadFirstL));
                    break;
                case NullState.ReadFirstL:
                    NextChar('l', _ => new NullParser(NullState.ReadSecondL));
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