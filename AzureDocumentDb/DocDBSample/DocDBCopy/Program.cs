namespace DocDBCopy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Core;
    using DocDBCopy.Model;

    public class Program
    {
        private const string DataPath = @"Data\Sample.csv";
        private const string Endpoint = "";
        private const string PrimaryKey = "";
        private const string DatabaseName = "";
        private const string CollectionName = "";

        public static void Main(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var inputPath = Path.Combine(basePath, DataPath);

            var contents = CsvUtility.ReadAllLines<Sample>(inputPath);

            var groups = contents.GroupBy(col => col.Column1);

            using (var client = new DocumentDbClient(Endpoint, PrimaryKey))
            {
                // TODO : consider TPL for faster creation.
                foreach (var group in groups)
                {
                    client.CreateDocument(DatabaseName, CollectionName, BuildDocument(group));
                }
            }

            Console.ReadLine();
        }

        private static IDictionary<string, object> BuildDocument(IGrouping<string, Sample> group)
        {
            var document = new Dictionary<string, object>();
            var sample = group.FirstOrDefault();

            document.Add(nameof(sample.Column1), sample.Column1);
            document.Add(nameof(sample.Column2), sample.Column2);

            var domains = group
                .GroupBy(col => col.Column3)
                .ToDictionary(col => col.Key, col => col.Count());

            foreach (var domain in domains)
            {
                document.Add(domain.Key, domain.Value);
            }

            return document;
        }
    }
}
