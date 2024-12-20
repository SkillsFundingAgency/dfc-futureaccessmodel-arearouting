﻿using System;
using DFC.FutureAccessModel.AreaRouting.Wrappers;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    /// <summary>
    /// i create document clients
    /// </summary>
    public interface ICreateDocumentClients
    {
        /// <summary>
        /// create client...
        /// </summary>
        /// <param name="forEndpoint">for endpoint</param>
        /// <param name="usingAccountKey">using account key</param>
        /// <returns>a newly constructed document client</returns>
        IWrapDocumentClient CreateClient(Uri forEndpoint, string usingAccountKey);
    }
}
