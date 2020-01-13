using System;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// the environment variable provider
    /// </summary>
    internal sealed class ApplicationSettingsProvider :
        IProvideApplicationSettings
    {
        /// <summary>
        /// get (the) variable
        /// </summary>
        /// <param name="usingTheValuesKey">using the values key</param>
        /// <returns>the value string</returns>
        public string GetVariable(string usingTheValuesKey) =>
            Environment.GetEnvironmentVariable(usingTheValuesKey);
    }
}
