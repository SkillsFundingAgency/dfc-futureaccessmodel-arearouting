namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// postcode client factory
    /// </summary>
    internal sealed class PostcodeClientFactory :
        ICreatePostcodeClients
    {
        /// <summary>
        /// create...
        /// </summary>
        /// <returns>a postcodes client wrapper</returns>
        public IWrapPostcodesClient Create() =>
            new PostcodesClientWrapper();
    }
}
