using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store documents
    /// </summary>
    public interface IStoreDocuments
    {
        /// <summary>
        /// document exists...
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>true if the document exists</returns>
        Task<bool> DocumentExists<TDocument>(Uri usingStoragePath, string andPartitionKey)
            where TDocument : class;

        /// <summary>
        /// add (a) document (to the document store)
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="theDocument">the document</param>
        /// <param name="usingCollectionPath">using (the) collection path</param>
        /// <returns>the submitted document</returns>
        Task<TDocument> AddDocument<TDocument>(TDocument theDocument, Uri usingCollectionPath)
            where TDocument : class;

        /// <summary>
        /// get document
        /// </summary>
        /// <typeparam name="TDocument">of this type</typeparam>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>the requested document</returns>
        Task<TDocument> GetDocument<TDocument>(Uri usingStoragePath, string andPartitionKey)
            where TDocument : class;

        /// <summary>
        /// delete document...
        /// </summary>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <param name="andPartitionKey">and partition key</param>
        /// <returns>the currently running task</returns>
        Task DeleteDocument(Uri usingStoragePath, string andPartitionKey);

        /// <summary>
        /// creat document query
        /// </summary>
        /// <typeparam name="TReturn">for return type</typeparam>
        /// <param name="usingCollection">using (the) collection (path)</param>
        /// <param name="andSQLCommand">and SQL command (default: select * from c)</param>
        /// <returns>the result of the command</returns>
        Task<IReadOnlyCollection<TReturn>> CreateDocumentQuery<TReturn>(Uri usingCollection, string andSQLCommand = "select * from c");
    }
}