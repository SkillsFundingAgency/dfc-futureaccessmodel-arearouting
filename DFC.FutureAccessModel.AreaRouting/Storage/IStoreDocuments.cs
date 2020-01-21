using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store documents
    /// </summary>
    public interface IStoreDocuments :
        ISupportServiceRegistration
    {
        /// <summary>
        /// add document
        /// </summary>
        /// <typeparam name="TDocument">of this type</typeparam>
        /// <param name="theDocument">the document</param>
        /// <param name="usingStoragePath">using (the) storage path</param>
        /// <returns></returns>
        Task AddDocument<TDocument>(TDocument theDocument, Uri usingStoragePath)
            where TDocument : class;

        /// <summary>
        /// get document
        /// </summary>
        /// <typeparam name="TDocument">of this type</typeparam>
        /// <param name="usingStoragePath">using the storage path</param>
        /// <returns></returns>
        Task<TDocument> GetDocument<TDocument>(Uri usingStoragePath)
            where TDocument : class;
    }
}