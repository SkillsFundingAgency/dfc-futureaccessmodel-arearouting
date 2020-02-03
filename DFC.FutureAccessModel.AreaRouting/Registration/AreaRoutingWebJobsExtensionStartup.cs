using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DFC.Functions.DI.Standard;
using DFC.FutureAccessModel.AreaRouting.Registration.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

namespace DFC.FutureAccessModel.AreaRouting.Registration
{
    /// <summary>
    /// area routing web jobs extension startup
    /// this can't be tested because the service collection utilises routine
    /// grafting that cannot be 'substituted' (static dpeendency)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class AreaRoutingWebJobsExtensionStartup :
        IWebJobsStartup
    {
        /// <summary>
        /// configure uses the service registrar to ensure complete service registration. 
        /// i'd like this to be injectable but i don't have control at this level. 
        /// so i have to use a static factory
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();

            var registrar = ServiceRegistrationProvider.CreateService(Assembly.GetExecutingAssembly());

            registrar.Compose(builder.Services);
        }
    }
}