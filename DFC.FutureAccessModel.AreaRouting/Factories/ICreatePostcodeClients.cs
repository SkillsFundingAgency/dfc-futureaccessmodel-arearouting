using DFC.FutureAccessModel.AreaRouting.Registration;

namespace DFC.FutureAccessModel.AreaRouting.Factories
{
    /// <summary>
    /// i create postcodes clients
    /// </summary>
    public interface ICreatePostcodeClients :
        ISupportServiceRegistration
    {
        /// <summary>
        /// create...
        /// </summary>
        /// <returns>a postcodes client wrapper</returns>
        IWrapPostcodesClient Create();
    }
}
