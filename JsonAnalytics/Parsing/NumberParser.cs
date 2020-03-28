using System;

namespace JsonAnalytics.Parsing
{
    public class NumberParser : JsonParser
    {
        private readonly NumberState _state;

        public enum NumberState
        {
            IsZero,
            ReadyForFirstDigit,
            SecondaryDigit,
            ReadyForFraction,
            ReadyToContinueFraction,
            ReadyForExponentStart,
            ReadyToContinueExponent
        }
        
        public NumberParser(NumberState state)
        {
            _state = state;
            switch (state)
            {
                case NumberState.IsZero:
                    NextChar(StructuralChar.DecimalSeparator, () => new NumberParser(NumberState.ReadyForFraction));
                    NextChar(StructuralChar.ScientificNotationSeparator, () => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForFirstDigit:
                    NextChar(StructuralChar.LeadingIntegerDigit, () => new NumberParser(NumberState.SecondaryDigit));
                    NextChar(StructuralChar.OnlyZero, () => new NumberParser(NumberState.IsZero));
                    break;
                case NumberState.SecondaryDigit:
                    NextChar(StructuralChar.FollowingIntegerDigit, () => new NumberParser(NumberState.SecondaryDigit));
                    NextChar(StructuralChar.DecimalSeparator, () => new NumberParser(NumberState.ReadyForFraction));
                    NextChar(StructuralChar.ScientificNotationSeparator, () => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForFraction:
                    NextChar(StructuralChar.FollowingIntegerDigit, () => new NumberParser(NumberState.ReadyToContinueFraction));
                    break;
                case NumberState.ReadyToContinueFraction:
                    NextChar(StructuralChar.FollowingIntegerDigit, () => new NumberParser(NumberState.ReadyToContinueFraction));
                    NextChar(StructuralChar.ScientificNotationSeparator, () => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForExponentStart:
                    NextChar(StructuralChar.FollowingIntegerDigit, () => new NumberParser(NumberState.ReadyToContinueExponent));
                    break;
                case NumberState.ReadyToContinueExponent:
                    NextChar(StructuralChar.FollowingIntegerDigit, () => new NumberParser(NumberState.ReadyToContinueExponent));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state switch
        {
            NumberState.IsZero => true,
            NumberState.ReadyForFirstDigit => false,
            NumberState.SecondaryDigit => true,
            NumberState.ReadyForFraction => false,
            NumberState.ReadyToContinueFraction => true,
            NumberState.ReadyForExponentStart => false,
            NumberState.ReadyToContinueExponent => true,
        };
    }
}