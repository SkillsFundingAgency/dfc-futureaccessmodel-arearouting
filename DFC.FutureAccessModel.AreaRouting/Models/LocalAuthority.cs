using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// the local authority (intermediary class)
    /// </summary>
    public class LocalAuthority :
        ILocalAuthority
    {
        /// <summary>
        /// the local admin district code
        /// </summary>
        [JsonProperty("id")]
        public string LADCode { get; set; }

        /// <summary>
        /// the (authorities) touchpoint
        /// </summary>
        public string TouchpointID { get; set; }

        /// <summary>
        /// the (authority) name
        /// </summary>
        public string Name { get; set; }
    }
}
