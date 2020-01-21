using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Adapters
{
    /// <summary>
    /// i get area routing detail...
    /// </summary>
    public interface IGetAreaRoutingDetails :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inLoggingScope);

        /// <summary>
        /// get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inLoggingScope);
    }
}