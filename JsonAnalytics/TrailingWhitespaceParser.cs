namespace JsonAnalytics
{
    public class TrailingWhitespaceParser : JsonParser
    {
        public TrailingWhitespaceParser()
        {
            NextChar(StructuralChar.Whitespace, () => this);
        }
        
        public override bool CanComplete => true;
        protected override bool IsNotNeededToComplete => true;
    }
}