using DFC.FutureAccessModel.AreaRouting.Models;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i analyse expressions
    /// </summary>
    public interface IAnalyseExpresssions
    {
        /// <summary>
        /// get type of expression for...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the expression type</returns>
        TypeOfExpression GetTypeOfExpressionFor(string theCandidate);
    }
}
