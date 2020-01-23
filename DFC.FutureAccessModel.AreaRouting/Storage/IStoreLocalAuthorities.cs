using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Storage
{
    /// <summary>
    /// i store local authorities
    /// </summary>
    public interface IStoreLocalAuthorities :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get...
        /// </summary>
        /// <param name="theAdminDistrict">the admin distict (code)</param>
        /// <returns>a local authority</returns>
        Task<ILocalAuthority> Get(string theAdminDistrict);

        /// <summary>
        /// add...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the newly added local authority</returns>
        Task<ILocalAuthority> Add(ILocalAuthority theCandidate);
    }
}