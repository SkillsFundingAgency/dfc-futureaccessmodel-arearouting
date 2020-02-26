namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// the local authority (intermediary)
    /// </summary>
    public interface ILocalAuthority
    {
        /// <summary>
        /// the local admin district code
        /// </summary>
        string LADCode { get; }

        /// <summary>
        /// the (authorities) touchpoint
        /// </summary>
        string TouchpointID { get; }

        /// <summary>
        /// the (authority) name
        /// </summary>
        string Name { get; }
    }
}
