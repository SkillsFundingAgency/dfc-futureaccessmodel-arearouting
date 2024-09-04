using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

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
            HttpRequest theRequest,
            ILogger usingTraceWriter,
            Func<IScopeLoggingContext, Task<IActionResult>> actionDo)
        {
            It.IsNull(theRequest)
                .AsGuard<ArgumentNullException>(nameof(theRequest));
            It.IsNull(usingTraceWriter)
                .AsGuard<ArgumentNullException>(nameof(usingTraceWriter));

            using (var inScope = await Factory.BeginScopeFor(theRequest, usingTraceWriter))
            {
                return await actionDo(inScope);
            }
        }
    }
}