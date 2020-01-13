using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Functions.DI.Standard.Attributes;
using DFC.FutureAccessModel.AreaRouting.Adapters;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.Swagger.Standard.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    public static class GetAreaRoutingDetailByTouchpointIDFunction
    {
        [FunctionName("GetAreaRoutingDetailByTouchpointID")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = "The Routing Detail was found", ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = "The Routing Detail does not exist for the given Touchpoint", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = "Request was malformed", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = "API key is unknown or invalid", ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = "Insufficient access", ShowSchema = false)]
        [Display(Name = "Get", Description = "Ability to return a Routing Detail for the given Touchpoint.")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas/{touchpointID}")]HttpRequest theRequest,
            string touchpointID,
            [Inject] ICreateLoggingContextScopes logging,
            [Inject] IGetAreaRoutingDetailByTouchpointID adapter)
        {
            using (var scope = await logging.BeginScopeFor(theRequest))
            {
                return await adapter.GetAreaRoutingDetailFor(touchpointID, scope);
            }
        }
    }
}