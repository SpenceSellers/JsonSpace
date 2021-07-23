using System;

namespace JsonAnalytics.Parsing
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
                    NextCanBeValueReturningTo(() => new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return));
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore whitespace
                    NextChar(StructuralChar.ArrayEnd, () => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
                    break;
                case ArrayState.ReadyForNext:
                    NextCanBeValueReturningTo(() => new ArrayParser(ArrayState.JustReadValue).ReturningTo(Return));
                    break;
                case ArrayState.JustReadValue:
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore whitespace
                    NextChar(StructuralChar.Comma, () => new ArrayParser(ArrayState.ReadyForNext).ReturningTo(Return));
                    NextChar(StructuralChar.ArrayEnd, () => new ArrayParser(ArrayState.Completed).ReturningTo(Return));
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