using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AuthorizationLevel = Microsoft.Azure.Functions.Worker.AuthorizationLevel;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    public sealed class PostAreaRoutingDetailFunction :
        AreaRoutingDetailFunction
    {
        private readonly ILogger<PostAreaRoutingDetailFunction> _logger;

        /// <summary>
        /// initialises an instance of the <see cref="PostAreaRoutingDetailFunction"/>
        /// </summary>
        /// <param name="factory">the logging scope factory</param>
        /// <param name="adapter">the area routing detail management function adapter</param>
        /// <param name="logger">The logger instance</param>
        public PostAreaRoutingDetailFunction(
            ICreateLoggingContextScopes factory,
            IManageAreaRoutingDetails adapter,
            ILogger<PostAreaRoutingDetailFunction> logger) : base(factory, adapter)
        {
            _logger = logger;
        }

        /// <summary>
        /// add area routing detail using...
        /// </summary>
        /// <param name="request">the request</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the resulting message</returns>
        public async Task<IActionResult> AddAreaRoutingDetailUsing(HttpRequest request, IScopeLoggingContext inScope)
        {
            var content = await request.ReadAsStringAsync();
            return await Adapter.AddAreaRoutingDetailUsing(content, inScope);
        }

        /// <summary>
        /// run...
        /// </summary>
        /// <param name="request">the request</param>
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "areas")]HttpRequest request) =>
                await RunActionScope(request, _logger, x => AddAreaRoutingDetailUsing(request, x));
    }
}