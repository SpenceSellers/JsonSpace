namespace JsonAnalytics.Parsing
{
    public class RootParser : JsonParser
    {
        public RootParser()
        {
            // Any possible JSON value can follow the empty string.
            // After that, any amount of trailing whitespace can follow that value.
            NextCanBeValueReturningTo(() => new TrailingWhitespaceParser());
        }

        // This isn't a trick question, an empty string is not a valid JSON value.
        public override bool CanComplete => false;

        protected override void AssignReturn(JsonParser nextParser)
        {
            // We want to go to trailing whitespace
        }
    }
}