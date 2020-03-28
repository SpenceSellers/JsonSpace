namespace JsonAnalytics.Parsing
{
    public class RootParser : JsonParser
    {
        public RootParser()
        {
            NextCanBeValueReturningTo(() => new TrailingWhitespaceParser());
        }

        public override bool CanComplete => false;

        protected override void AssignReturn(JsonParser nextParser)
        {
            // We want to go to trailing whitespace
        }
    }
}