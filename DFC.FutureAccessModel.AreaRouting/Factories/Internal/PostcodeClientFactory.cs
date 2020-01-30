using DFC.FutureAccessModel.AreaRouting.Wrappers;
using DFC.FutureAccessModel.AreaRouting.Wrappers.Internal;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// postcode client factory
    /// </summary>
    internal sealed class PostcodeClientFactory :
        ICreatePostcodeClients
    {
        /// <summary>
        /// create client...
        /// </summary>
        /// <returns>a postcodes client wrapper</returns>
        public IWrapPostcodesClient CreateClient() =>
            new PostcodesClientWrapper();
    }
}
