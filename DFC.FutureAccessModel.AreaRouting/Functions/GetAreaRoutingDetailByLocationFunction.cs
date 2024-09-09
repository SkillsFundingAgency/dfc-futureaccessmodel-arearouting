using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// get area routing detail by location function
    /// </summary>
    public sealed class GetAreaRoutingDetailByLocationFunction :
        AreaRoutingDetailFunction
    {
        private readonly ILogger<GetAreaRoutingDetailByLocationFunction> _logger;

        /// <summary>
        /// (the http request query) location key
        /// </summary>
        private const string LocationKey = "location";

        /// <summary>
        /// initialises an instance of the <see cref="GetAreaRoutingDetailByLocationFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        /// <param name="logger">The logger instance</param>
        public GetAreaRoutingDetailByLocationFunction(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<GetAreaRoutingDetailByLocationFunction> logger) : base(factory, adapter)
        {
            _logger = logger;
        }

        /// <summary>
        /// do request...
        /// </summary>
        /// <param name="request">the request</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the http message response</returns>
        public async Task<IActionResult> DoRequest(HttpRequest request, IScopeLoggingContext inScope)
        {
            var hasSelector = request.Query.ContainsKey(LocationKey);
            var location = request.Query[LocationKey];

            return hasSelector
                ? await Adapter.GetAreaRoutingDetailBy(location, inScope)
                : await Adapter.GetAllRouteIDs(inScope);
        }

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="request">the request</param>
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas")]
            HttpRequest request) =>
                await RunActionScope(request, _logger, x => DoRequest(request, x));
    }
}