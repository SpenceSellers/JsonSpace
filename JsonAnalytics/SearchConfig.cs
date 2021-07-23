using System;

namespace JsonAnalytics
{
    /// <summary>
    /// Defines a Breadth First Search across the space of all JSON possible strings.
    /// </summary>
    public class SearchConfig
    {
        public Func<JsonHandler.BfsNode, bool> IsSuccessState = _ => false;
        public Func<JsonHandler.BfsNode, bool> CanLeadToSuccessState = _ => true;
    }
}