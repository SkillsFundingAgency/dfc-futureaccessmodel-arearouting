using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    public abstract class AreaRoutingDetailFunction
    {
        public ICreateLoggingContextScopes Factory { get; }

        public IManageAreaRoutingDetails Adapter { get; }

        protected AreaRoutingDetailFunction(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter)
        {
            It.IsNull(factory)
                .AsGuard<ArgumentNullException>(nameof(factory));
            It.IsNull(adapter)
                .AsGuard<ArgumentNullException>(nameof(adapter));

            Factory = factory;
            Adapter = adapter;
        }

        public async Task<IActionResult> RunActionScope(
            HttpRequest request,
            ILogger usingTraceWriter,
            Func<IScopeLoggingContext, Task<IActionResult>> actionDo)
        {
            It.IsNull(request)
                .AsGuard<ArgumentNullException>(nameof(request));
            It.IsNull(usingTraceWriter)
                .AsGuard<ArgumentNullException>(nameof(usingTraceWriter));

            using (var inScope = await Factory.BeginScopeFor(request, usingTraceWriter))
            {
                return await actionDo(inScope);
            }
        }
    }
}