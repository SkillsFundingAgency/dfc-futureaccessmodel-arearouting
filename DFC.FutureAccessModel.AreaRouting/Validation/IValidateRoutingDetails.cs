using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Validation
{
    /// <summary>
    /// i validate routing details
    /// </summary>
    public interface IValidateRoutingDetails :
        ISupportServiceRegistration
    {
        /// <summary>
        /// validate...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the currently running task</returns>
        Task Validate(IRoutingDetail theCandidate);
    }
}