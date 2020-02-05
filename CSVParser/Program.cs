using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace CSVParser
{
     class Program
    {   
        static void Main(string[] args)
        {   
            CSVParser parser = new CSVParser();
            string json = parser.Parse("../data/input_file.txt");
            Console.WriteLine(json);
            ParseToFile(json);
        }

        public static string ParseToFile(string jsonresponse) {
            File.WriteAllText("../data/output.json", jsonresponse);
            return jsonresponse;
        }
    }
}