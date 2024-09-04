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
    /// the delete area rouing detail by touchpoint id function
    /// </summary>
    public sealed class DeleteAreaRoutingDetailByTouchpointIDFunction :
        AreaRoutingDetailFunction
    {
        /// <summary>
        /// initialises an instance of the <see cref="DeleteAreaRoutingDetailByTouchpointIDFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        public DeleteAreaRoutingDetailByTouchpointIDFunction(ICreateLoggingContextScopes factory, IManageAreaRoutingDetails adapter) : base(factory, adapter) { }

        /// <summary>
        /// delete an area routing detail using...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <param name="inScope">in scope</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAreaRoutingDetailUsing(string theTouchpoint, IScopeLoggingContext inScope) =>
            await Adapter.DeleteAreaRoutingDetailUsing(theTouchpoint, inScope);

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "areas/{touchpointID}")]HttpRequest theRequest,
            ILogger usingTraceWriter,
            string touchpointID) =>
                await RunActionScope(theRequest, usingTraceWriter, x => DeleteAreaRoutingDetailUsing(touchpointID, x));
    }
}