using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Adapters.Internal;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Factories.Internal;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Providers.Internal;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Storage.Internal;
using DFC.FutureAccessModel.AreaRouting.Validation;
using DFC.FutureAccessModel.AreaRouting.Validation.Internal;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using DFC.FutureAccessModel.AreaRouting.Wrappers.Internal;
using DFC.Swagger.Standard;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWebApplication()
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;
                    services.AddOptions<ConfigurationSettings>()
                        .Bind(configuration);

                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddLogging();
                    services.AddSingleton<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
                    services.AddSingleton<IManageAreaRoutingDetails, AreaRoutingDetailManagementFunctionAdapter>();
                    services.AddSingleton<ICreatePostcodeClients, PostcodeClientFactory>();
                    services.AddSingleton<ICreateLoggingContextScopes, LoggingContextScopeFactory>();
                    services.AddSingleton<ICreateValidationMessageContent, ValidationMessageContentFactory>();
                    services.AddSingleton<IAnalyseExpresssions, ExpressionAnalyser>();
                    services.AddSingleton<IProvideApplicationSettings, ApplicationSettingsProvider>();
                    services.AddSingleton<IProvideExpressionActions, ExpressionActionProvider>();
                    services.AddSingleton<IProvideFaultResponses, FaultResponseProvider>();
                    services.AddSingleton<IProvideSafeOperations, SafeOperationsProvider>();
                    services.AddSingleton<IStoreAreaRoutingDetails, AreaRoutingDetailStore>();
                    services.AddSingleton<IStoreLocalAuthorities, LocalAuthorityStore>();
                    services.AddSingleton<IValidateRoutingDetails, RoutingDetailValidator>();
                    services.AddTransient<IWrapCosmosDbClient, CosmosDbWrapper>();

                    services.AddSingleton(s =>
                    {
                        var settings = s.GetRequiredService<IOptions<ConfigurationSettings>>().Value;
                        var options = new CosmosClientOptions() { ConnectionMode = ConnectionMode.Gateway };

                        return new CosmosClient(settings.DocumentStoreEndpointAddress, settings.DocumentStoreAccountKey, options);
                    });

                    services.Configure<LoggerFilterOptions>(options =>
                    {
                        LoggerFilterRule toRemove = options.Rules.FirstOrDefault(rule => rule.ProviderName
                            == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
                        if (toRemove is not null)
                        {
                            options.Rules.Remove(toRemove);
                        }
                    });
                })
                .Build();
            await host.RunAsync();
        }
    }
}
