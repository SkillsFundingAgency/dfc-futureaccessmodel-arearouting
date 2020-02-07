using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Adapters
{
    /// <summary>
    /// i manage area routing details
    /// </summary>
    public interface IManageAreaRoutingDetails :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inScope);

        /// <summary>
        /// get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inScope);

        /// <summary>
        /// add area routing detail using
        /// </summary>
        /// <param name="theContent">the content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<HttpResponseMessage> AddAreaRoutingDetailUsing(string theContent, IScopeLoggingContext inScope);
    }
}