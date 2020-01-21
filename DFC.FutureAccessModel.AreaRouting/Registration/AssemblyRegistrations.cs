using DFC.Common.Standard.Logging;
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
using DFC.Swagger.Standard;
using MarkEmbling.PostcodesIO;
using Microsoft.Azure.WebJobs.Hosting;

// the web job extension startup registration
[assembly: WebJobsStartup(typeof(AreaRoutingWebJobsExtensionStartup), "Area Routing Web Jobs Extension Startup")]

// inherited, package level
[assembly: ExternalRegistration(typeof(IPostcodesIOClient), typeof(PostcodesIOClient), TypeOfRegistrationScope.Singleton)]
[assembly: ExternalRegistration(typeof(ILoggerHelper), typeof(LoggerHelper), TypeOfRegistrationScope.Singleton)]
[assembly: ExternalRegistration(typeof(IHttpResponseMessageHelper), typeof(HttpResponseMessageHelper), TypeOfRegistrationScope.Singleton)]
[assembly: ExternalRegistration(typeof(ISwaggerDocumentGenerator), typeof(SwaggerDocumentGenerator), TypeOfRegistrationScope.Singleton)]

// might need to put these back at some point
// builder.Services.AddSingleton<IHttpRequestHelper, HttpRequestHelper>()
// builder.Services.AddSingleton<IJsonHelper, JsonHelper>()

// project level
// adapters
[assembly: InternalRegistration(typeof(IGetAreaRoutingDetails), typeof(GetAreaRoutingDetailFunctionAdapter), TypeOfRegistrationScope.Singleton)]

// factories
[assembly: InternalRegistration(typeof(ICreateDocumentClients), typeof(DocumentClientFactory), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(ICreateLoggingContextScopes), typeof(LoggingContextScopeFactory), TypeOfRegistrationScope.Singleton)]

// providers
[assembly: InternalRegistration(typeof(IAnalyseExpresssions), typeof(ExpressionAnalyser), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IProvideApplicationSettings), typeof(ApplicationSettingsProvider), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IProvideExpressionActions), typeof(ExpressionActionProvider), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IProvideFaultResponses), typeof(FaultResponseProvider), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IProvideSafeOperations), typeof(SafeOperationsProvider), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IProvideStoragePaths), typeof(StoragePathProvider), TypeOfRegistrationScope.Singleton)]

// storage
[assembly: InternalRegistration(typeof(IStoreAreaRoutingDetails), typeof(AreaRoutingDetailStore), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IStoreLocalAuthorities), typeof(LocalAuthorityStore), TypeOfRegistrationScope.Singleton)]
[assembly: InternalRegistration(typeof(IStoreDocuments), typeof(DocumentStore), TypeOfRegistrationScope.Singleton)]
