using DFC.FutureAccessModel.AreaRouting.Models;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Validation
{
    /// <summary>
    /// i validate routing details
    /// </summary>
    public interface IValidateRoutingDetails
    {
        /// <summary>
        /// validate...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the currently running task</returns>
        Task Validate(IRoutingDetail theCandidate);
    }
}