using System;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CSVParser.Tests
{
    public class CSVParserTests
    {
        private CSVParser _parser;
        public CSVParserTests()
        {
            _parser = new CSVParser();
        }

        [Fact]
        public void IsEmpty()
        {
            string path = "../data/input_file.txt";
            var output = _parser.Parse(path);
            Assert.NotEmpty(output);
        }

        [Fact]
        public void TestContent()
        {
            string path = "../data/input_file.txt";
            var output = _parser.Parse(path);
            Assert.IsType<String>(output);
        }

        [Fact]
        public void TestOrders()
        {
            string path = "../data/input_file.txt";
            var jsonoutput = _parser.Parse(path);
            var data = JObject.Parse(jsonoutput);
            Assert.Equal(4, data.Count);
        }
    }
}
