using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    /// <summary>
    /// Defines a Breadth First Search across the space of all JSON possible strings.
    /// </summary>
    public class SearchConfig
    {
        public List<StructuralChar> InitialState = new();
        public Func<JsonHandler.BfsNode, bool> IsSuccessState = _ => false;
        public Func<JsonHandler.BfsNode, bool> CanLeadToSuccessState = _ => true;
    }
}