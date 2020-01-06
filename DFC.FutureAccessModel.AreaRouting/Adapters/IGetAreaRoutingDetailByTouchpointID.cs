using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        /// <param name="theRequest">the request</param>
        /// <param name="usingTouchpointID">using the touchpoint id</param>
        /// <returns>the response message which will contain the routing details for a satisfactory request</returns>
        Task<HttpResponseMessage> GetAreaRoutingDetailFor(HttpRequest theRequest, string usingTouchpointID);
    }
}