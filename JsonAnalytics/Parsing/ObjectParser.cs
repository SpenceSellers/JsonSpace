using System;

namespace JsonAnalytics.Parsing
{
    public class ObjectParser : JsonParser
    {
        private readonly ObjectState _state;

        public enum ObjectState
        {
            ReadyForFirstKey,
            ReadyForKey,
            ReadyForColon,
            ReadyForValue,
            ReadyForNext, // from here go to } or Value
            Completed
        }

        public ObjectParser() : this(ObjectState.ReadyForFirstKey)
        {
        }

        public ObjectParser(ObjectState state)
        {
            _state = state;
            switch (state)
            {
                case ObjectState.ReadyForFirstKey:
                    NextChar(StructuralChar.StringDelimiter, () => new StringParser().ReturningTo(new ObjectParser(ObjectState.ReadyForColon).ReturningTo(Return)));
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore it
                    NextChar(StructuralChar.ObjectEnd, () => new ObjectParser(ObjectState.Completed).ReturningTo(Return));
                    break;
                case ObjectState.ReadyForKey:
                    NextChar(StructuralChar.StringDelimiter, () => new StringParser().ReturningTo(new ObjectParser(ObjectState.ReadyForColon).ReturningTo(Return)));
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore it
                    break;
                case ObjectState.ReadyForColon:
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore it
                    NextChar(StructuralChar.KeyValueSeparator, () => new ObjectParser(ObjectState.ReadyForValue).ReturningTo(Return));
                    break;
                case ObjectState.ReadyForValue:
                    NextCanBeValueReturningTo(() => new ObjectParser(ObjectState.ReadyForNext).ReturningTo(Return));
                    break;
                case ObjectState.ReadyForNext:
                    NextChar(StructuralChar.Whitespace, () => this); // Ignore it
                    NextChar(StructuralChar.ObjectEnd, () => new ObjectParser(ObjectState.Completed).ReturningTo(Return));
                    NextChar(StructuralChar.Comma, () => new ObjectParser(ObjectState.ReadyForKey).ReturningTo(Return));
                    break;
                case ObjectState.Completed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public override bool CanComplete => _state == ObjectState.Completed;

        protected override void AssignReturn(JsonParser nextParser)
        {
            // We'll manually handle it
        }
    }
}