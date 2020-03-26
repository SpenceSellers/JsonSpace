namespace JsonAnalytics
{
    public class RootParser : JsonParser
    {
        public RootParser()
        {
            NextCanBeValueReturningTo(() => null);
        }

        public override bool CanComplete => false;
    }
}