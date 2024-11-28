using System.Collections.Generic;
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
        /// get...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <returns>an area routing detail (the touchpoint)</returns>
        Task<IRoutingDetail> Get(string theTouchpoint);

        /// <summary>
        /// get all routing detail id's
        /// </summary>
        /// <returns>the full list of routing detail id's</returns>
        Task<IReadOnlyCollection<string>> GetAllIDs();

        /// <summary>
        /// add...
        /// </summary>
        /// <param name="theCandidate">the candidate (touchpoint)</param>
        /// <returns>the newly stored routing details (touchpoint)</returns>
        Task<IRoutingDetail> Add(IncomingRoutingDetail theCandidate);

        /// <summary>
        /// delete...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <returns>an area routing detail (the touchpoint)</returns>
        Task Delete(string theTouchpoint);
    }
}