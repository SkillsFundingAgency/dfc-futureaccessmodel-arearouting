using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Wrappers
{
    public interface IWrapCosmosDbClient
    {
        Task<List<RoutingDetail>> GetAllAreaRoutingsAsync();
        Task<RoutingDetail> GetAreaRoutingDetailAsync(string theTouchpoint);
        Task<LocalAuthority> GetLocalAuthorityAsync(string theAdminDistrict);
        Task<bool> AreaRoutingDetailExistsAsync(string id, string partitionKey);
        Task<ItemResponse<RoutingDetail>> CreateAreaRoutingDetailAsync(RoutingDetail routingDetail, string partitionKey);
        Task<ItemResponse<RoutingDetail>> DeleteAreaRoutingDetailAsync(string theTouchpoint, string partitionKey);
    }
}