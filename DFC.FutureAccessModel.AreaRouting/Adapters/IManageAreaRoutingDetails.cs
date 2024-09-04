using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using Microsoft.AspNetCore.Mvc;

namespace DFC.FutureAccessModel.AreaRouting.Adapters
{
    /// <summary>
    /// i manage area routing details
    /// </summary>
    public interface IManageAreaRoutingDetails
    {
        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<IActionResult> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inScope);

        /// <summary>
        /// get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<IActionResult> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inScope);

        /// <summary>
        /// add area routing detail using
        /// </summary>
        /// <param name="theContent">the content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<IActionResult> AddAreaRoutingDetailUsing(string theContent, IScopeLoggingContext inScope);

        /// <summary>
        /// get all route id's
        /// </summary>
        /// <param name="inScope">in scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<IActionResult> GetAllRouteIDs(IScopeLoggingContext inScope);

        /// <summary>
        /// delete an area routing detail using...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        Task<IActionResult> DeleteAreaRoutingDetailUsing(string theTouchpointID, IScopeLoggingContext inScope);
    }
}