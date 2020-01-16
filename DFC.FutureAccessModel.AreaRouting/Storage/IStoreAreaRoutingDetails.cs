using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store area routing details
    /// </summary>
    public interface IStoreAreaRoutingDetails
    {
        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <returns>an area routing detail</returns>
        Task<IRoutingDetail> GetAreaRoutingDetailFor(string theTouchpointID);
    }
}