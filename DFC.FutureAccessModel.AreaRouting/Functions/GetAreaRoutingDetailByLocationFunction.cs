using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Functions.DI.Standard.Attributes;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    public static class GetAreaRoutingDetailByLocationFunction
    {
        /// <summary>
        /// run...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
        /// <param name="factory">(the logging scope) factory</param>
        /// <param name="adapter">(the routing details) adapter</param>
        /// <returns>the http response to the operation</returns>
        [FunctionName("GetAreaRoutingDetailByLocation")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = FunctionDescription.ResourceFound, ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Get an Area Routing Detail By Location", Description =
            @"Ability to return:<br />
                a list of Touchpoint ID's<br />
                or a singluar full area routing detail when coupled with the use of the location parameter<br />
                Examples:<br />
                <ul>
                    <li>?location=TS14 6AH</li>
                    <li>?location=Stafford (search by town proposed, not yet implemented)</li>
                    <li>?location=WS11 (search by outward code proposed, not yet implemented)</li>
                </ul>")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas")]HttpRequest theRequest,
            ILogger usingTraceWriter,
            [Inject] ICreateLoggingContextScopes factory,
            [Inject] IManageAreaRoutingDetails adapter)
        {
            It.IsNull(theRequest)
                .AsGuard<ArgumentNullException>(nameof(theRequest));
            It.IsNull(usingTraceWriter)
                .AsGuard<ArgumentNullException>(nameof(usingTraceWriter));
            It.IsNull(factory)
                .AsGuard<ArgumentNullException>(nameof(factory));
            It.IsNull(adapter)
                .AsGuard<ArgumentNullException>(nameof(adapter));

            using (var scope = await factory.BeginScopeFor(theRequest, usingTraceWriter))
            {
                const string _locationKey = "location";

                var hasSelector = theRequest.Query.ContainsKey(_locationKey);
                var theLocation = theRequest.Query[_locationKey];

                return hasSelector
                    ? await adapter.GetAreaRoutingDetailBy(theLocation, scope)
                    : await adapter.GetAllRouteIDs(scope);
            }
        }
    }
}