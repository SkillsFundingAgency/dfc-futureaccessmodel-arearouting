using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    public interface IProvideDocumentStorage
    {
        Task<IRoutingDetail> GetAreaRoutingDetail(string usingTouchpointID);

        Task<bool> DoesAreaRoutingDetailExistFor(string theTouchpointID);
    }
}