using DFC.Common.Standard.Logging;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Adapters.Internal;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Factories.Internal;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Providers.Internal;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Storage.Internal;
using DFC.FutureAccessModel.AreaRouting.Validation;
using DFC.FutureAccessModel.AreaRouting.Validation.Internal;
using DFC.Swagger.Standard;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddLogging();
        services.AddSingleton<ILoggerHelper, LoggerHelper>();
        services.AddSingleton<ISwaggerDocumentGenerator, SwaggerDocumentGenerator>();
        services.AddSingleton<IManageAreaRoutingDetails, AreaRoutingDetailManagementFunctionAdapter>();
        services.AddSingleton<ICreatePostcodeClients, PostcodeClientFactory>();
        services.AddSingleton<ICreateDocumentClients, DocumentClientFactory>();
        services.AddSingleton<ICreateLoggingContextScopes, LoggingContextScopeFactory>();
        services.AddSingleton<ICreateValidationMessageContent, ValidationMessageContentFactory>();
        services.AddSingleton<IAnalyseExpresssions, ExpressionAnalyser>();
        services.AddSingleton<IProvideApplicationSettings, ApplicationSettingsProvider>();
        services.AddSingleton<IProvideExpressionActions, ExpressionActionProvider>();
        services.AddSingleton<IProvideFaultResponses, FaultResponseProvider>();
        services.AddSingleton<IProvideSafeOperations, SafeOperationsProvider>();
        services.AddSingleton<IProvideStoragePaths, StoragePathProvider>();
        services.AddSingleton<IStoreAreaRoutingDetails, AreaRoutingDetailStore>();
        services.AddSingleton<IStoreLocalAuthorities, LocalAuthorityStore>();
        services.AddSingleton<IStoreDocuments, DocumentStore>();
        services.AddSingleton<IValidateRoutingDetails, RoutingDetailValidator>();
    })
    .ConfigureLogging(logging =>
    {
        // The Application Insights SDK adds a default logging filter that instructs ILogger to capture only Warning and more severe logs. Application Insights requires an explicit override.
        // For more information, see https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=windows#application-insights
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });
    })
    .Build();

host.Run();
