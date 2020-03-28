using NUnit.Framework;

namespace JsonAnalytics.Tests
{
    public class JsonParserTests
    {
        // Valid
        [TestCase("100", ExpectedResult = true)]
        [TestCase("1000000", ExpectedResult = true)]
        [TestCase("[1,2,3,4]", ExpectedResult = true)]
        [TestCase("[1, 2, 3, 4]", ExpectedResult = true)]
        [TestCase("      100", ExpectedResult = true)]
        [TestCase("100     ", ExpectedResult = true)]
        [TestCase("[11,2]", ExpectedResult = true)]
        [TestCase("[11  ,2]", ExpectedResult = true)]
        [TestCase("[  11,2]", ExpectedResult = true)]
        [TestCase("[11,  2]", ExpectedResult = true)]
        [TestCase("[11,2  ]", ExpectedResult = true)]
        [TestCase("[]", ExpectedResult = true)]
        [TestCase("[[1,2,3,4]]", ExpectedResult = true)]
        [TestCase("[1, [2, 3], 4]", ExpectedResult = true)]
        [TestCase("-150", ExpectedResult = true)]
        [TestCase("-0", ExpectedResult = true)]
        [TestCase("0", ExpectedResult = true)]
        [TestCase("1.6e10", ExpectedResult = true)]
        [TestCase("1.6", ExpectedResult = true)]
        [TestCase("1.0", ExpectedResult = true)]
        [TestCase("0.0", ExpectedResult = true)]
        [TestCase("5e5", ExpectedResult = true)]
        [TestCase("1e0", ExpectedResult = true)]
        [TestCase("0e0", ExpectedResult = true)]
        [TestCase("[1.1,2e2,3.3e3,4e40]", ExpectedResult = true)]
        [TestCase("[1,2,null,3]", ExpectedResult = true)]
        [TestCase("null", ExpectedResult = true)]
        [TestCase(@"""test""", ExpectedResult = true)]
        [TestCase(@"  ""test""", ExpectedResult = true)]
        [TestCase(@"""test""  ", ExpectedResult = true)]
        [TestCase(@"""t\tst""", ExpectedResult = true)]
        [TestCase(@"""t\nst""", ExpectedResult = true)]
        [TestCase(@"""t\u29c8continue""", ExpectedResult = true)]
        [TestCase(@"[""test""]", ExpectedResult = true)]
        [TestCase("{\"key\": \"value\"}", ExpectedResult = true)]
        [TestCase("{\"key\":1, \"key2\": 2}", ExpectedResult = true)]
        [TestCase("{\"key\":1 ,\"key2\":2 }", ExpectedResult = true)]
        [TestCase("[ ]", ExpectedResult = true)]
        
        // Invalid
        [TestCase("[1,2,3,4", ExpectedResult = false)]
        [TestCase("[", ExpectedResult = false)]
        [TestCase("1,2,3,4", ExpectedResult = false)]
        [TestCase("]", ExpectedResult = false)]
        [TestCase("[1[2]", ExpectedResult = false)]
        [TestCase("[1[2]    ", ExpectedResult = false)]
        [TestCase("    [1[2]", ExpectedResult = false)]
        [TestCase("[[[[[[120]", ExpectedResult = false)]
        [TestCase("01", ExpectedResult = false)]
        [TestCase("-01", ExpectedResult = false)]
        [TestCase("1.6e10e2", ExpectedResult = false)]
        [TestCase("1e2e3", ExpectedResult = false)]
        [TestCase("1e2.2", ExpectedResult = false)]
        [TestCase("1e", ExpectedResult = false)]
        [TestCase("1.", ExpectedResult = false)]
        [TestCase("nall", ExpectedResult = false)]
        [TestCase(@"""Test", ExpectedResult = false)]
        [TestCase(@"""t\qst""", ExpectedResult = false)]
        [TestCase(@"""t\u29cshort""", ExpectedResult = false)]
        public bool CanTellIfJsonIsValid(string input)
        {
            return new JsonHandler().IsValidJson(input);
        }
    }
}