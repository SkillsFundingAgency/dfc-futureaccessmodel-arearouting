using System.Diagnostics.CodeAnalysis;
using DFC.Common.Standard.Logging;
using DFC.Functions.DI.Standard;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Adapters.Internal;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Factories.Internal;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Providers.Internal;
using DFC.FutureAccessModel.AreaRouting.Registration;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Storage.Internal;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;
using DFC.Swagger.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(AreaRoutingWebJobsExtensionStartup), "Area Routing Web Jobs Extension Startup")]
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
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();

            // inherited, package level
            builder.Services.AddSingleton<ILoggerHelper, LoggerHelper>();
            builder.Services.AddSingleton<IHttpResponseMessageHelper, HttpResponseMessageHelper>();
            builder.Services.AddSingleton<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();

            // might need to put these back at some point
            // builder.Services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>()
            // builder.Services.AddSingleton<IJsonHelper, JsonHelper>()

            // project level
            // adapters
            builder.Services.AddSingleton<IGetAreaRoutingDetails, GetAreaRoutingDetailFunctionAdapter>();

            // factories
            builder.Services.AddSingleton<ICreateDocumentClients, DocumentClientFactory>();
            builder.Services.AddSingleton<ICreateLoggingContextScopes, LoggingContextScopeFactory>();

            // providers
            builder.Services.AddSingleton<IProvideApplicationSettings, ApplicationSettingsProvider>();
            builder.Services.AddSingleton<IProvideFaultResponses, FaultResponseProvider>();
            builder.Services.AddSingleton<IProvideSafeOperations, SafeOperationsProvider>();

            // storage
            builder.Services.AddSingleton<IStoreAreaRoutingDetails, AreaRoutingDetailStore>();
            builder.Services.AddSingleton<IStoreDocuments, DocumentStore>();
            builder.Services.AddSingleton<IProvideStoragePaths, StoragePathProvider>();
        }
    }
}