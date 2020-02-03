using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i analyse expressions
    /// </summary>
    public interface IAnalyseExpresssions :
        ISupportServiceRegistration
    {
        /// <summary>
        /// get type of expression for...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the expression type</returns>
        TypeOfExpression GetTypeOfExpressionFor(string theCandidate);
    }
}
