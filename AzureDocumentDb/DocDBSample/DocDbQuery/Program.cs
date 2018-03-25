namespace DocDbQuery
{
    using System;
    using System.Collections.Generic;

    using Core;
    using Newtonsoft.Json;

    public class Program
    {
        private const string Endpoint = "";
        private const string PrimaryKey = "";
        private const string DatabaseName = "";
        private const string CollectionName = "";
        private const string FilterKey = "_";

        public static void Main(string[] args)
        {
            using (var client = new DocumentDbClient(Endpoint, PrimaryKey))
            {
                var response = client
                    .ReadDocument<IDictionary<string, object>>(DatabaseName, CollectionName, "<documentid>")
                    .RemoveRange(FilterKey);

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            }

            Console.ReadLine();
        }
    }
}
