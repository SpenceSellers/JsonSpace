using System;

namespace JsonAnalytics
{
    public class SearchConfig
    {
        public Func<JsonHandler.BfsNode, bool> IsSuccessState = _ => false;
        public Func<JsonHandler.BfsNode, bool> CanLeadToSuccessState = _ => true;
    }
}