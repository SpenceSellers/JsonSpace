using System;

namespace JsonAnalytics.Parsing
{
    public class TrueParser : JsonParser
    {

        private readonly TrueState _state;

        public enum TrueState
        {
            ReadT,
            ReadR,
            ReadU,
            ReadE
        }

        public TrueParser(TrueState state)
        {
            _state = state;
            switch (state)
            {
                case TrueState.ReadT:
                    NextChar(StructuralChar.TrueTwo, () => new TrueParser(TrueState.ReadR));
                    break;
                case TrueState.ReadR:
                    NextChar(StructuralChar.TrueThree, () => new TrueParser(TrueState.ReadU));
                    break;
                case TrueState.ReadU:
                    NextChar(StructuralChar.TrueFour, () => new TrueParser(TrueState.ReadE));
                    break;
                case TrueState.ReadE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == TrueState.ReadE;
    }
}