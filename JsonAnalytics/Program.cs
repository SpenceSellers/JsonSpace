using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var n = 0; n < 20; n++)
            {
                var s = "[";
                JsonParser parser = new RootParser().Read('[');
                for (var i = 0; i < 250; i++)
                {
                    // var nextChar = RandomElement(parser.AcceptableChars());
                    var nextChars = parser.AcceptableChars().ToList();
                    if (!nextChars.Any())
                    {
                        break;
                    }
            
                    var next = RandomElement(nextChars);
                    s += next;
                    parser = parser.Read(next);
                }
                
                Console.Out.WriteLine(s);
            }
            
            // new JsonHandler().Bfs();
        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }
    }
}