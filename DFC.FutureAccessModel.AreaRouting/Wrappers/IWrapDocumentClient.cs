using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers
{
    /// <summary>
    /// i document client (shim)
    /// </summary>
    public interface IWrapDocumentClient
    {
        /// <summary>
        /// create document (async)
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="usingCollectionPath">using collection path</param>
        /// <param name="andCandidate">the candidate (document)</param>
        /// <returns>the stored document</returns>
        Task<TDocument> CreateDocumentAsync<TDocument>(Uri usingCollectionPath, TDocument andCandidate)
            where TDocument : class;

        /// <summary>
        /// document exists (async)
        /// this will throw if the document does not exist
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>true if it does</returns>
        Task<bool> DocumentExistsAsync<TDocument>(Uri usingStoragePath, string andPartitionKey)
            where TDocument : class;

        /// <summary>
        /// read document (async)
        /// throws if the document does not exist
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>an instance of the requested type <typeparamref name="TDocument"/></returns>
        Task<TDocument> ReadDocumentAsync<TDocument>(Uri usingStoragePath, string andPartitionKey)
            where TDocument : class;

        /// <summary>
        /// delete document (async)
        /// throws if the document does not exist
        /// </summary>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>the running task</returns>
        Task DeleteDocumentAsync(Uri usingStoragePath, string andPartitionKey);

        /// <summary>
        /// create document query...
        /// </summary>
        /// <typeparam name="TReturn">for return type</typeparam>
        /// <param name="usingCollection">using (the) collection (path)</param>
        /// <param name="andSQLCommand">and SQL command</param>
        /// <returns>a docuement query</returns>
        IDocumentQuery<TReturn> CreateDocumentQuery<TReturn>(Uri usingCollection, string andSQLCommand);
    }
}
