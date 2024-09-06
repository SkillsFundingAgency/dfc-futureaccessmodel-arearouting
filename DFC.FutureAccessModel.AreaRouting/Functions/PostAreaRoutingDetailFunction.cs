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
using Microsoft.Azure.WebJobs.Extensions.Http;
using AuthorizationLevel = Microsoft.Azure.Functions.Worker.AuthorizationLevel;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    public sealed class PostAreaRoutingDetailFunction :
        AreaRoutingDetailFunction
    {
        /// <summary>
        /// initialises an instance of the <see cref="PostAreaRoutingDetailFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        public PostAreaRoutingDetailFunction(ICreateLoggingContextScopes factory, IManageAreaRoutingDetails adapter) : base(factory, adapter) {}

        /// <summary>
        /// add area routing detail using...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the resulting message</returns>
        public async Task<IActionResult> AddAreaRoutingDetailUsing(HttpRequest theRequest, IScopeLoggingContext inScope)
        {
            var theContent = await theRequest.ReadAsStringAsync();
            return await Adapter.AddAreaRoutingDetailUsing(theContent, inScope);
        }

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="theRequest">the request</param>
        /// <param name="usingTraceWriter">using (the) trace writer</param>
        /// <returns>the http response to the operation</returns>
        [Function("PostAreaRoutingDetail")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Created, Description = FunctionDescription.ResourceCreated, ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoParentContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Conflict, Description = FunctionDescription.Conflict, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Create a new Area Routing Detail", Description = "Ability to add an Area Routing Detail.")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "areas")]HttpRequest theRequest,
            ILogger usingTraceWriter) =>
                await RunActionScope(theRequest, usingTraceWriter, x => AddAreaRoutingDetailUsing(theRequest, x));
    }
}