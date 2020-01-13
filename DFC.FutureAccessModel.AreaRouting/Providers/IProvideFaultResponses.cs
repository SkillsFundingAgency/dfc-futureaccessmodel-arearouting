using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i provide fault responses
    /// </summary>
    public interface IProvideFaultResponses
    {
        /// <summary>
        /// get (the) response for...
        /// </summary>
        /// <param name="theException">the exception</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the http response message</returns>
        Task<HttpResponseMessage> GetResponseFor(Exception theException, IScopeLoggingContext useLoggingScope);
    }
}
