using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store area routing details
    /// </summary>
    public interface IStoreAreaRoutingDetails
    {
        Task<IRoutingDetail> GetAreaRoutingDetailFor(string theTouchpointID);
    }
}