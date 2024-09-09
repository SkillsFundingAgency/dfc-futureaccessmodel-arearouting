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
    /// the delete area routing detail by touchpoint id function
    /// </summary>
    public sealed class DeleteAreaRoutingDetailByTouchpointIDFunction :
        AreaRoutingDetailFunction
    {
        private readonly ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction> _logger;

        /// <summary>
        /// initialises an instance of the <see cref="DeleteAreaRoutingDetailByTouchpointIDFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        /// <param name="logger">The logger instance</param>
        public DeleteAreaRoutingDetailByTouchpointIDFunction(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<DeleteAreaRoutingDetailByTouchpointIDFunction> logger) : base(factory, adapter)
        {
            _logger = logger;
        }

        /// <summary>
        /// delete an area routing detail using...
        /// </summary>
        /// <param name="touchpoint">the touchpoint (id)</param>
        /// <param name="inScope">in scope</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAreaRoutingDetailUsing(string touchpoint, IScopeLoggingContext inScope) =>
            await Adapter.DeleteAreaRoutingDetailUsing(touchpoint, inScope);

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="request">the request</param>
        /// <param name="touchpointID">(the) touchpoint id</param>
        /// <returns>the http response to the operation</returns>
        [Function("DeleteAreaRoutingDetail")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Delete an Area Routing Detail by ID", Description = "Ability to delete an Area Routing Detail for the given Touchpoint.")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "areas/{touchpointID}")]
            HttpRequest request,
            string touchpointID) =>
                await RunActionScope(request, _logger, x => DeleteAreaRoutingDetailUsing(touchpointID, x));
    }
}