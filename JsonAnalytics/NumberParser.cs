using System;

namespace JsonAnalytics
{
    public class NumberParser : JsonParser
    {
        private readonly NumberState _state;

        public static NumberParser GetNumberParser(char c)
        {
            if (c == '0')
            {
                return new NumberParser(NumberState.IsZero);
            }
            
            if (c == '-')
            {
                return new NumberParser(NumberState.ReadyForFirstDigit);
            }

            if ("123456789".Contains(c))
            {
                return new NumberParser(NumberState.SecondaryDigit);
            }
            
            throw new ArgumentException("Bad Number start: " + c);
        }
        
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
                    break;
                case NumberState.ReadyForFirstDigit:
                    NextChar("123456789", _ => new NumberParser(NumberState.SecondaryDigit));
                    break;
                case NumberState.SecondaryDigit:
                    NextChar("0123456789", _ => new NumberParser(NumberState.SecondaryDigit));
                    NextChar(".", _ => new NumberParser(NumberState.ReadyForFraction));
                    NextChar("eE", _ => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForFraction:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueFraction));
                    break;
                case NumberState.ReadyToContinueFraction:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueFraction));
                    NextChar("eE", _ => new NumberParser(NumberState.ReadyForExponentStart));
                    break;
                case NumberState.ReadyForExponentStart:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueExponent));
                    break;
                case NumberState.ReadyToContinueExponent:
                    NextChar("0123456789", _ => new NumberParser(NumberState.ReadyToContinueExponent));
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