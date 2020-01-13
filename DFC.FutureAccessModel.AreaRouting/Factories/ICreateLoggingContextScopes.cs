using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    /// <summary>
    /// i create logging context scopes
    /// </summary>
    public interface ICreateLoggingContextScopes
    {
        /// <summary>
        /// begin scope
        /// </summary>
        /// <returns>a new logging scope</returns>
        Task<IScopeLoggingContext> BeginScopeFor(HttpRequest theRequest, [CallerMemberName] string initialisingRoutine = null);
    }
}
