using System;

namespace JsonAnalytics
{
    public class FalseParser : JsonParser
    {
        private readonly FalseState _state;

        public enum FalseState
        {
            ReadF,
            ReadA,
            ReadL,
            ReadS,
            ReadE
        }
        
        public FalseParser(FalseState state)
        {
            _state = state;
            switch (state)
            {
                case FalseState.ReadF:
                    NextChar(StructuralChar.FalseTwo, () => new FalseParser(FalseState.ReadA));
                    break;
                case FalseState.ReadA:
                    NextChar(StructuralChar.FalseThree, () => new FalseParser(FalseState.ReadL));
                    break;
                case FalseState.ReadL:
                    NextChar(StructuralChar.FalseFour, () => new FalseParser(FalseState.ReadS));
                    break;
                case FalseState.ReadS:
                    NextChar(StructuralChar.FalseFive, () => new FalseParser(FalseState.ReadE));
                    break;
                case FalseState.ReadE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == FalseState.ReadE;
    }
}