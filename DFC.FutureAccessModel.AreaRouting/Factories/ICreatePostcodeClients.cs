using DFC.FutureAccessModel.AreaRouting.Registration;
using DFC.FutureAccessModel.AreaRouting.Wrappers;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    /// <summary>
    /// i create postcodes clients
    /// </summary>
    public interface ICreatePostcodeClients :
        ISupportServiceRegistration
    {
        /// <summary>
        /// create client...
        /// </summary>
        /// <returns>a postcodes client wrapper</returns>
        IWrapPostcodesClient CreateClient();
    }
}
