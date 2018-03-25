namespace Core
{
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;

    public static class CsvUtility
    {
        public static IList<T> ReadAllLines<T>(string path)
        {
            var result = new List<T>();

            using (var fileReader = File.OpenText(path))
            using (var csvReader = new CsvReader(fileReader))
            { 
                while (csvReader.Read())
                {
                    result.Add(csvReader.GetRecord<T>());
                }
            }

            return result;
        }
    }
}
