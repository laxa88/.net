﻿namespace DocDBCopy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
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
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            var basePath = Directory.GetCurrentDirectory();
            var inputPath = Path.Combine(basePath, DataPath);

            var contents = CsvUtility.ReadAllLines<Sample>(inputPath);
            var groups = contents.GroupBy(col => col.Column1);
            var client = new DocumentDbClient(Endpoint, PrimaryKey);
            var fullCollect = true;

            long memory = 0;
            var initialMemory = GC.GetTotalMemory(fullCollect);
            Console.WriteLine($"### memory: {initialMemory}, {timer.ElapsedMilliseconds}");

            // parallel
            //Parallel.ForEach(groups, (group, state, index) =>
            //{
            //    memory = GC.GetTotalMemory(fullCollect) - initialMemory;
            //    Console.WriteLine($"START {index} memory: {memory}, {timer.ElapsedMilliseconds}");

            //    client.CreateDocument(DatabaseName, CollectionName, BuildDocument(group));

            //    memory = GC.GetTotalMemory(fullCollect) - initialMemory;
            //    Console.WriteLine($"DONE {index} memory: {memory}, {timer.ElapsedMilliseconds}");
            //});

            // concurrent
            //for (var i = 0; i < groups.Count(); i++)
            //{
            //    var index = i;
            //    var group = groups.ElementAt(index);

            //    memory = GC.GetTotalMemory(fullCollect) - initialMemory;
            //    Console.WriteLine($"START {index} memory: {memory}, {timer.ElapsedMilliseconds}");

            //    var newTask = new Task(() =>
            //    {
            //        client.CreateDocument(DatabaseName, CollectionName, BuildDocument(group));
            //    });

            //    newTask.Start();

            //    newTask.ContinueWith((t) =>
            //    {
            //        memory = GC.GetTotalMemory(fullCollect) - initialMemory;
            //        Console.WriteLine($"DONE {index} memory: {memory}, {timer.ElapsedMilliseconds}");
            //    });
            //}

            memory = GC.GetTotalMemory(fullCollect) - initialMemory;
            Console.WriteLine($"### memory: {memory}, {timer.ElapsedMilliseconds}");

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
