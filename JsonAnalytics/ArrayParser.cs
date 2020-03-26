using System;

namespace JsonAnalytics
{
    public class ArrayParser : JsonParser
    {
        private readonly ArrayState _state;

        public enum ArrayState
        {
            ReadyForFirst,
            ReadyForNext,
            JustReadValue,
            Completed
        }
        
        public ArrayParser(ArrayState state)
        {
            _state = state;
            switch (state)
            {
                case ArrayState.ReadyForFirst:
                    // NextChar(ValueParser.ValueStarts, c => ValueParser.ParserForValue(c).ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return)));
                    NextCanBeValueReturningTo(() => new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return));
                    NextChar(StructuralChar.Whitespace, _ => this); // Ignore it
                    NextChar(StructuralChar.ArrayEnd, _ => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    break;
                case ArrayState.ReadyForNext:
                    // NextChar(ValueParser.ValueStarts, c => ValueParser.ParserForValue(c).ReturningTo(new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return)));
                    NextCanBeValueReturningTo(() => new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return));
                    break;
                case ArrayState.JustReadValue:
                    NextChar(StructuralChar.Whitespace, _ => this); // Ignore it
                    NextChar(StructuralChar.Comma, _ => new ArrayParser(ArrayState.ReadyForNext).ReturningTo(Return));
                    NextChar(StructuralChar.ArrayEnd, _ => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    break;
                case ArrayState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == ArrayState.Completed;

        protected override void AssignReturn(JsonParser nextParser)
        {
            // We'll manually handle it
        }
    }
}