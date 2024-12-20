﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DFC.Common.Standard.Logging;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DFC.FutureAccessModel.AreaRouting.Factories.Internal
{
    /// <summary>
    /// the logging context scope factory
    /// </summary>
    internal sealed class LoggingContextScopeFactory :
        ICreateLoggingContextScopes
    {
        /// <summary>
        /// the header version key name
        /// </summary>
        public const string HeaderVersion = "Version";

        /// <summary>
        /// the header api key name
        /// </summary>
        public const string HeaderAPIKey = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// the value for an unfound key item
        /// </summary>
        public const string ValueNotFound = "(not found)";

        /// <summary>
        /// the DFC logging helper
        /// </summary>
        public ILoggerHelper LoggerHelper { get; }

        /// <summary>
        /// initalises a new instance of the <see cref="LoggingContextScopeFactory"/>
        /// </summary>
        /// <param name="logHelper">the DFC logging helper</param>
        public LoggingContextScopeFactory(ILoggerHelper logHelper)
        {
            It.IsNull(logHelper)
                .AsGuard<ArgumentNullException>(nameof(logHelper));

            LoggerHelper = logHelper;
        }

        /// <summary>
        /// begin scope
        /// </summary>
        /// <param name="request">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
        /// <param name="initialisingRoutine">initalising routine</param>
        /// <returns>a new logging scope</returns>
        public async Task<IScopeLoggingContext> BeginScopeFor(
            HttpRequest request,
            ILogger usingTraceWriter,
            [CallerMemberName] string initialisingRoutine = null)
        {
            var scope = new LoggingContextScope(usingTraceWriter, LoggerHelper, initialisingRoutine);

            await scope.Information($"beginning scope for remote host, address: '{request.HttpContext.Connection.RemoteIpAddress}' port: '{request.HttpContext.Connection.RemotePort}'");
            await scope.Information($"request verb: '{request.Method}'");
            await scope.Information($"request content type: '{request.ContentType}'");
            await scope.Information($"request query string: '{request.QueryString}'");
            await scope.Information($"requested API Version: '{GetHeaderItemFrom(request, HeaderVersion)}'");
            await scope.Information($"request API Key: '{GetHeaderItemFrom(request, HeaderAPIKey)}'");

            return scope;
        }

        /// <summary>
        /// get (a) header item from...
        /// </summary>
        /// <param name="request">the request</param>
        /// <param name="headerItem">the header item</param>
        /// <returns>the header item value or a default string '(not found)'</returns>
        internal string GetHeaderItemFrom(HttpRequest request, string headerItem) =>
            request.Headers.ContainsKey(headerItem)
                ? request.Headers[headerItem].FirstOrDefault()
                : ValueNotFound;
    }
}
