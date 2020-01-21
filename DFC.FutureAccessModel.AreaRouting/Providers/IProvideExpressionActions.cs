using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Providers
{
    /// <summary>
    /// i provide expression actions
    /// </summary>
    public interface IProvideExpressionActions :
        ISupportServiceRegistration
    {
        /// <summary>
        ///  get action for
        /// </summary>
        /// <param name="theExpressionType">the expression type</param>
        /// <returns>
        /// the action for the expression type or
        /// throws <seealso cref="Faults.MalformedRequestException"/>
        /// for unknown types</returns>
        Func<string, IScopeLoggingContext, Task<string>> GetActionFor(TypeOfExpression theExpressionType);
    }
}
