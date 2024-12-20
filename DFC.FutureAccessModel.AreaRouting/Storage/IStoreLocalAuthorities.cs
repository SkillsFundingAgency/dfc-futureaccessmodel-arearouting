using DFC.FutureAccessModel.AreaRouting.Models;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store local authorities
    /// </summary>
    public interface IStoreLocalAuthorities
    {
        /// <summary>
        /// get...
        /// </summary>
        /// <param name="theAdminDistrict">the admin distict (code)</param>
        /// <returns>a local authority</returns>
        Task<ILocalAuthority> Get(string theAdminDistrict);
    }
}