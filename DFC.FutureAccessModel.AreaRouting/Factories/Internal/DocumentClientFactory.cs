using System;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// the document client factory
    /// </summary>
    internal sealed class DocumentClientFactory :
        ICreateDocumentClients
    {
        /// <summary>
        /// create client...
        /// </summary>
        /// <param name="forEndpoint">for endpoint</param>
        /// <param name="usingAccountKey">using account key</param>
        /// <returns>a newly constructed document client</returns>
        public IDocumentClientShim CreateClient(Uri forEndpoint, string usingAccountKey) =>
            new StorageClient(forEndpoint, usingAccountKey);
    }
}
