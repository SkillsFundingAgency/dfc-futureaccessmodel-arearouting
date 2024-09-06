using System.ComponentModel.DataAnnotations;
using System.Net;
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
    public sealed class GetAreaRoutingDetailByTouchpointIDFunction :
        AreaRoutingDetailFunction
    {
        /// <summary>
        /// initialises an instance of the <see cref="GetAreaRoutingDetailByTouchpointIDFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        public GetAreaRoutingDetailByTouchpointIDFunction(ICreateLoggingContextScopes factory, IManageAreaRoutingDetails adapter) : base(factory, adapter) { }

        /// <summary>
        /// get area routing detail for...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="inScope">in scope</param>
        /// <returns>a message result</returns>
        public async Task<IActionResult> GetAreaRoutingDetailFor(string theTouchpoint, IScopeLoggingContext inScope) =>
            await Adapter.GetAreaRoutingDetailFor(theTouchpoint, inScope);

        /// <summary>
        /// run...
        /// touchpoint details (2020-01-16)
        /// 0000000101, East of England and Buckinghamshire
        /// 0000000102, East Midlands and Northamptonshire
        /// 0000000103, London
        /// 0000000104, West Midlands
        /// 0000000105, North West
        /// 0000000106, North East and Cumbria
        /// 0000000107, South East
        /// 0000000108, South West
        /// 0000000109, Yorkshire and Humber
        /// 0000000999, National Careers Helpline
        /// 1000000000, Digital
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
        /// <param name="touchpointID">(the) touchpoint id</param>
        /// <returns>the http response to the operation</returns>
        [Function("GetAreaRoutingDetailByTouchpointID")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = FunctionDescription.ResourceFound, ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Get an Area Routing Detail By ID", Description = "Ability to return a Routing Detail for the given Touchpoint.")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas/{touchpointID}")]HttpRequest theRequest,
            ILogger usingTraceWriter,
            string touchpointID) =>
                await RunActionScope(theRequest, usingTraceWriter, x => GetAreaRoutingDetailFor(touchpointID, x));
    }
}