using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonAnalytics
{
    class Program
    {
        static void Main(string[] args)
        {
            var p1 = new ValueParser();
            var p2 = p1.Read('[');
            var p3 = p2.Read('1');
            var p4 = p3.Read(']');
            //
            // var nexts = string.Join("", p4.AcceptableChars());
            // Console.Out.WriteLine(nexts);

            // for (var n = 0; n < 20; n++)
            // {
            //     var s = "[";
            //     JsonParser parser = new ValueParser().Read('[');
            //     for (var i = 0; i < 2500; i++)
            //     {
            //         // var nextChar = RandomElement(parser.AcceptableChars());
            //         var nextChars = parser.AcceptableChars().ToList();
            //         if (!nextChars.Any())
            //         {
            //             break;
            //         }
            //
            //         var next = RandomElement(nextChars);
            //         s += next;
            //         parser = parser.Read(next);
            //     }
            //     
            //     Console.Out.WriteLine(s);
            // }
            
            new JsonHandler().Bfs();
        }

        private static T RandomElement<T>(IList<T> items)
        {
            var i = new Random().Next(items.Count);
            return items[i];
        }
    }
}