using NUnit.Framework;

namespace JsonAnalytics.Tests
{
    public class Tests
    {
        [TestCase("100", ExpectedResult = true)]
        [TestCase("1000000", ExpectedResult = true)]
        [TestCase("[1,2,3,4]", ExpectedResult = true)]
        [TestCase("[]", ExpectedResult = true)]
        [TestCase("[[1,2,3,4]]", ExpectedResult = true)]
        
        [TestCase("[1,2,3,4", ExpectedResult = false)]
        [TestCase("[", ExpectedResult = false)]
        [TestCase("1,2,3,4", ExpectedResult = false)]
        [TestCase("]", ExpectedResult = false)]
        [TestCase("[1[2]", ExpectedResult = false)]
        public bool CanTellIfJsonIsValid(string input)
        {
            return new JsonHandler().IsValidJson(input);
        }
    }
}