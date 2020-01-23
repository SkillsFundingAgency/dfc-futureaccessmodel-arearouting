using System;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.HTTP.Standard;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    /// <summary>
    /// a general (base) function adapter
    /// </summary>
    internal abstract class GeneralFunctionAdapter
    {
        /// <summary>
        /// the fault (response provider)
        /// </summary>
        public IProvideFaultResponses Faults { get; }

        /// <summary>
        /// the safe operations (provider)
        /// </summary>
        public IProvideSafeOperations SafeOperations { get; }

        /// <summary>
        /// the response (helper)
        /// </summary>
        public IHttpResponseMessageHelper Respond { get; }

        /// <summary>
        /// create an instance of <see cref="GeneralFunctionAdapter"/>
        /// </summary>
        /// <param name="responseHelper">the response helper</param>
        /// <param name="faultResponses">the fault responses (provider)</param>
        /// <param name="safeOperations">the safe operations (provider)</param>
        protected GeneralFunctionAdapter(
            IHttpResponseMessageHelper responseHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations)
        {
            It.IsNull(responseHelper)
                .AsGuard<ArgumentNullException>(nameof(responseHelper));
            It.IsNull(faultResponses)
                .AsGuard<ArgumentNullException>(nameof(faultResponses));
            It.IsNull(safeOperations)
                .AsGuard<ArgumentNullException>(nameof(safeOperations));

            Respond = responseHelper;
            Faults = faultResponses;
            SafeOperations = safeOperations;
        }
    }
}