using System;
using System.Net;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using Microsoft.Azure.Documents;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    internal sealed class DocumentStore :
        IStoreDocuments
    {
        /// <summary>
        /// the document store endpoint address key
        /// </summary>
        public const string DocumentStoreEndpointAddressKey = "DocumentStoreEndpointAddress";

        /// <summary>
        /// the document store account key
        /// </summary>
        public const string DocumentStoreAccountKey = "DocumentStoreAccountKey";

        /// <summary>
        /// the endpoint address
        /// </summary>
        public string EndpointAddress { get; }

        /// <summary>
        /// the 'azure' account key
        /// </summary>
        public string AccountKey { get; }

        /// <summary>
        /// the document store client
        /// </summary>
        public IDocumentClient Client { get; }

        public IProvideSafeOperations SafeOperations { get; }

        /// <summary>
        /// initialises an instance of the document store
        /// </summary>
        /// <param name="usingEnvironment">using environment variables</param>
        public DocumentStore(
            IProvideApplicationSettings usingEnvironment,
            ICreateDocumentClients factory,
            IProvideSafeOperations safeOperations)
        {
            It.IsNull(usingEnvironment)
                .AsGuard<ArgumentNullException>(nameof(usingEnvironment));
            It.IsNull(factory)
                .AsGuard<ArgumentNullException>(nameof(factory));
            It.IsNull(safeOperations)
                .AsGuard<ArgumentNullException>(nameof(safeOperations));

            EndpointAddress = usingEnvironment.GetVariable(DocumentStoreEndpointAddressKey);
            It.IsEmpty(EndpointAddress)
                .AsGuard<ArgumentNullException>(nameof(EndpointAddress));

            AccountKey = usingEnvironment.GetVariable(DocumentStoreAccountKey);
            It.IsEmpty(AccountKey)
                .AsGuard<ArgumentNullException>(nameof(AccountKey));

            Client = factory.CreateClient(new Uri(EndpointAddress), AccountKey);
            SafeOperations = safeOperations;
        }

        /// <summary>
        /// add (a) document (to the document store)
        /// </summary>
        /// <typeparam name="TDocument">the docuement type</typeparam>
        /// <param name="theDocument">the document</param>
        /// <param name="usingStoragePath">using the storage path</param>
        /// <returns>the currently running task</returns>
        public async Task AddDocument<TDocument>(TDocument theDocument, Uri usingStoragePath)
            where TDocument : class =>
            await Client.CreateDocumentAsync(usingStoragePath, theDocument);

        /// <summary>
        /// get (a) document (from the document store)
        /// </summary>
        /// <typeparam name="TDocument">the docuement type</typeparam>
        /// <param name="usingStoragePath">using the storage path</param>
        /// <returns>the currently running task</returns>
        public async Task<TDocument> GetDocument<TDocument>(Uri usingStoragePath)
            where TDocument : class =>
            await SafeOperations.Try(
                () => ProcessGetDocument<TDocument>(usingStoragePath),
                x => ProcessGetDocumentErrorHandler<TDocument>(x as DocumentClientException));

        /// <summary>
        /// process get document
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="usingStoragePath"></param>
        /// <returns></returns>
        internal async Task<TDocument> ProcessGetDocument<TDocument>(Uri usingStoragePath)
            where TDocument : class
        {
            var doc = await Client.ReadDocumentAsync(usingStoragePath);
            return await doc.Resource.ConvertTo<TDocument>() ?? default;
        }

        /// <summary>
        /// process get document error handler.
        /// an incoming null is likely to be the result of an argument null
        /// exception for a null uri in the read dosument call
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="theException"></param>
        /// <returns></returns>
        internal async Task<TDocument> ProcessGetDocumentErrorHandler<TDocument>(DocumentClientException theException)
            where TDocument : class =>
            await Task.Run(() =>
            {
                if (It.IsNull(theException))
                {
                    throw new MalformedRequestException();
                }

                if (HttpStatusCode.NotFound == theException.StatusCode.Value)
                {
                    throw new NoContentException();
                }

                if (LocalHttpStatusCode.TooManyRequests.ComparesTo(theException.StatusCode))
                {
                    throw new MalformedRequestException();
                }

                // we don't expect to ever get here...
                return default(TDocument);
            });
    }
}
