using System;
using System.Collections.Generic;

namespace JsonAnalytics.BreadthFirstSearch
{
    /// <summary>
    /// Defines a Breadth First Search across the space of all JSON possible strings.
    /// </summary>
    public class SearchConfig
    {
        public List<StructuralChar> InitialState = new();
        public Func<BreadthFirstSearch.BfsNode, bool> IsSuccessState = _ => false;
        public Func<BreadthFirstSearch.BfsNode, bool> CanLeadToSuccessState = _ => true;
    }
}