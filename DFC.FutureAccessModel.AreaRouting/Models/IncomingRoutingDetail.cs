using DFC.FutureAccessModel.AreaRouting.Storage;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    public sealed class IncomingRoutingDetail :
        RoutingDetail
    {
        /// <summary>
        /// here to ensure cosmos db grouping is singular
        /// </summary>
        [PartitionKey]
        public string PartitionKey => "not_required";
    }
}