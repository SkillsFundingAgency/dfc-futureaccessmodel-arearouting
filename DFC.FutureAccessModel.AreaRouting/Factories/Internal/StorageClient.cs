using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Diagnostics.CodeAnalysis;
#if DEBUG
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
#else
using DFC.FutureAccessModel.AreaRouting.Helpers;
using Microsoft.Azure.Documents.Client;
#endif

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// the storage client is a document client 'shim' which
    /// enables limited cosmos 'emulation' in DEBUG mode
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal sealed class StorageClient :
            IDocumentClientShim
    {
#if DEBUG
        private readonly Dictionary<Uri, object> _store = new Dictionary<Uri, object>();

        private readonly Uri _endpointAddress;
        private readonly string _key;

        public StorageClient(Uri forEndpoint, string usingAccountKey)
        {
            _endpointAddress = forEndpoint;
            _key = usingAccountKey;
        }

        public async Task<TResource> CreateDocumentAsync<TResource>(Uri documentCollectionUri, TResource document)
            where TResource : class =>
            await Task.Run(() =>
            {
                var keyPath = MakeResourcePath(document, documentCollectionUri);

                if(_store.ContainsKey(keyPath))
                {
                    var ex = MakeDocumentClientException(HttpStatusCode.Conflict);
                    throw ex;
                }

                _store.Add(keyPath, document);

                return document;
            });

        public async Task<TResource> ReadDocumentAsync<TResource>(Uri documentUri)
            where TResource : class =>
            await Task.Run(() =>
            {
                if (!_store.ContainsKey(documentUri))
                {
                    var ex = MakeDocumentClientException(HttpStatusCode.NotFound);
                    throw ex;
                }

                return _store[documentUri] as TResource;
            });

        internal Uri MakeResourcePath<TResource>(TResource theResource, Uri collectionPath)
        {
            var keyValueName = theResource.GetType()
                .GetProperties()
                .FirstOrDefault(x => x.GetCustomAttribute<KeyAttribute>() != null);

            return new Uri($"{collectionPath.OriginalString}/docs/{keyValueName.GetValue(theResource)}", UriKind.Relative);
        }

        /// <summary>
        /// make (a) document client exception
        /// all constructors have been internalised for some reason so we can't mock or 'new' up
        /// </summary>
        /// <param name="httpStatusCode"></param>
        /// <returns>a document client exception</returns>
        internal DocumentClientException MakeDocumentClientException(HttpStatusCode httpStatusCode)
        {
            var type = typeof(DocumentClientException);
            var name = type.FullName;
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var parms = new object[] { new Error(), null, httpStatusCode };

            var instance = type.Assembly.CreateInstance(name, false, flags, null, parms, null, null);

            return (DocumentClientException)instance;
        }
#else
        private readonly IDocumentClient _client;

        public StorageClient(Uri forEndpoint, string usingAccountKey)
        {
            _client = new DocumentClient(forEndpoint, usingAccountKey);
        }

        public async Task<TDocument> CreateDocumentAsync<TDocument>(Uri documentCollectionUri, TDocument document)
            where TDocument : class
        {
            var doc = await _client.CreateDocumentAsync(documentCollectionUri, document);
            return await doc.Resource.ConvertTo<TDocument>() ?? default;
        }

        public async Task<TDocument> ReadDocumentAsync<TDocument>(Uri documentUri)
            where TDocument : class
        {
            var doc = await _client.ReadDocumentAsync<TDocument>(documentUri);
            return doc.Document;
        }
#endif
    }
}
