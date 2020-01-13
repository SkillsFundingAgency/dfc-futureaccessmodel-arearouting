using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;

namespace DFC.FutureAccessModel.AreaRouting.Adapters
{
    /// <summary>
    /// i get area routing detail by touchpoint id
    /// </summary>
    public interface IGetAreaRoutingDetailByTouchpointID
    {
        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext useLoggingScope);
    }
}