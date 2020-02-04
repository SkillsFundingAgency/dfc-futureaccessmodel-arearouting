using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// the routing detail contract
    /// </summary>
    public interface IRoutingDetail
    {
        /// <summary>
        /// the region identifier
        /// </summary>
        string TouchpointID { get; }

        /// <summary>
        /// the name of the region
        /// </summary>
        string Area { get; }

        /// <summary>
        /// contractor's region specific contact phone number
        /// </summary>
        string TelephoneNumber { get; }

        /// <summary>
        /// contractor's region specific contact 'text' number
        /// </summary>
        string SMSNumber { get; }

        /// <summary>
        /// contractor's region specific contact email address
        /// </summary>
        string EmailAddress { get; }
    }
}