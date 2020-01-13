using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

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
        public IDocumentClient CreateClient(Uri forEndpoint, string usingAccountKey) =>
            new DocumentClient(forEndpoint, usingAccountKey);
    }
}
