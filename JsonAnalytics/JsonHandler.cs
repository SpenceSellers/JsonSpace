using System.Linq;

namespace JsonAnalytics
{
    public class JsonHandler
    {
        public bool IsValidJson(string json)
        {
            JsonParser parser = new ValueParser();
            foreach (var c in json)
            {
                if (!parser.AcceptableChars().Contains(c))
                {
                    return false;
                }
                
                parser = parser.Read(c);
            }

            return parser.CanBeTheEndOfInput;
        }
    }
}