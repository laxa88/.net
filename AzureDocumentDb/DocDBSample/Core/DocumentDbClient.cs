namespace Core
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    public class DocumentDbClient : IDisposable
    {
        private DocumentClient client;
        private bool disposed = false;

        public DocumentDbClient(string endpoint, string primaryKey)
        {
            this.client = new DocumentClient(new Uri(endpoint), primaryKey);
        }

        public void CreateDocument(string databaseName, string collectionName, object document)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("Client have been disposed.");
            }

            CreateDocumentAsync(databaseName, collectionName, document).Wait();
        }

        private async Task CreateDocumentAsync(string databaseName, string collectionName, object document)
        {
            Console.WriteLine("creating document...");
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), document);
            Console.WriteLine("created document.");
        }

        public T ReadDocument<T>(string databaseName, string collectionName, string documentId)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("Client have been disposed.");
            }

            return client
                .ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, documentId))
                .ContinueWith(
                    task =>
                    {
                        return (T)(dynamic)(Document)task.Result;
                    }).Result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }

                disposed = true;
            }
        }
    }
}