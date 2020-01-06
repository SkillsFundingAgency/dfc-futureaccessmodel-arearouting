using DFC.Common.Standard.Logging;
using DFC.Functions.DI.Standard;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Adapters.Internal;
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
    public sealed class AreaRoutingWebJobsExtensionStartup :
        IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection();

            builder.Services.AddSingleton<ILoggerHelper, LoggerHelper>();
            builder.Services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>();
            builder.Services.AddSingleton<IHttpResponseMessageHelper, HttpResponseMessageHelper>();
            builder.Services.AddSingleton<IJsonHelper, JsonHelper>();

            // TODO: check why this was added under a scope
            builder.Services.AddSingleton<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();

            builder.Services.AddSingleton<IProvideStorageAccess, StorageProvider>();
            builder.Services.AddSingleton<IGetAreaRoutingDetailByTouchpointID, GetAreaRoutingDetailByTouchpointIDFunctionAdapter>();
        }
    }
}