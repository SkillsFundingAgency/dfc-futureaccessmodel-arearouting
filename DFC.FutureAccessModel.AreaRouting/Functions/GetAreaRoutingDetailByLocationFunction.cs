using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// get area routing detail by location function
    /// </summary>
    public sealed class GetAreaRoutingDetailByLocationFunction :
        AreaRoutingDetailFunction
    {
        /// <summary>
        /// (the http request query) location key
        /// </summary>
        private const string LocationKey = "location";

        /// <summary>
        /// initialises an instance of the <see cref="GetAreaRoutingDetailByLocationFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        public GetAreaRoutingDetailByLocationFunction(ICreateLoggingContextScopes factory, IManageAreaRoutingDetails adapter) : base(factory, adapter) { }

        /// <summary>
        /// do request...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the http message response</returns>
        public async Task<IActionResult> DoRequest(HttpRequest theRequest, IScopeLoggingContext inScope)
        {
            var hasSelector = theRequest.Query.ContainsKey(LocationKey);
            var theLocation = theRequest.Query[LocationKey];

            return hasSelector
                ? await Adapter.GetAreaRoutingDetailBy(theLocation, inScope)
                : await Adapter.GetAllRouteIDs(inScope);
        }

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
        /// <param name="factory">(the logging scope) factory</param>
        /// <param name="adapter">(the routing details) adapter</param>
        /// <returns>the http response to the operation</returns>
        [Function("GetAreaRoutingDetailByLocation")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = FunctionDescription.ResourceFound, ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Get an Area Routing Detail By Location", Description =
            @"Ability to return:<br />
                a list of Touchpoint ID's<br />
                or a singular full area routing detail when coupled with the use of the 'location' parameter<br />
                Location based examples:<br />
                <ul>
                    <li>?location=TS14 6AH</li>
                    <li>?location=WS11 (search by outward code)</li>
                    <li>?location=Stafford (search by town proposed, not yet implemented)</li>
                </ul>")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas")]HttpRequest theRequest,
            ILogger usingTraceWriter) =>
                await RunActionScope(theRequest, usingTraceWriter, x => DoRequest(theRequest, x));
    }
}