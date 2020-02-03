using System;
using System.Threading.Tasks;

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
        /// <typeparam name="TResource"></typeparam>
        /// <param name="documentCollectionUri">the document collection path</param>
        /// <param name="document">the document</param>
        /// <returns>the stored document</returns>
        Task<TResource> CreateDocumentAsync<TResource>(Uri documentCollectionUri, TResource document)
            where TResource : class;

        /// <summary>
        /// document exists (async)
        /// </summary>
        /// <typeparam name="TDocument">the document type</typeparam>
        /// <param name="documentUri">the path to the document</param>
        /// <returns>true if it does</returns>
        Task<bool> DocumentExistsAsync<TDocument>(Uri documentUri)
            where TDocument : class;

        /// <summary>
        /// read document (async).
        /// throws if the document does not exist
        /// </summary>
        /// <typeparam name="TResource">the type of resource</typeparam>
        /// <param name="documentUri">the doucment path</param>
        /// <returns>an instance of the requested type <typeparamref name="TResource"/></returns>
        Task<TResource> ReadDocumentAsync<TResource>(Uri documentUri)
            where TResource : class;
    }
}
