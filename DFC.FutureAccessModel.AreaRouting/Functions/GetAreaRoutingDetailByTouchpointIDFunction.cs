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
    public static class GetAreaRoutingDetailByTouchpointIDFunction
    {
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
        /// <param name="touchpointID"></param>
        /// <param name="factory"></param>
        /// <param name="adapter"></param>
        /// <returns></returns>
        [FunctionName("GetAreaRoutingDetailByTouchpointID")]
        [ProducesResponseType(typeof(RoutingDetail), (int)HttpStatusCode.OK)]
        [Response(HttpStatusCode = (int)HttpStatusCode.OK, Description = FunctionDescription.ResourceFound, ShowSchema = true)]
        [Response(HttpStatusCode = (int)HttpStatusCode.NoContent, Description = FunctionDescription.NoContent, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.BadRequest, Description = FunctionDescription.MalformedRequest, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Unauthorized, Description = FunctionDescription.Unauthorised, ShowSchema = false)]
        [Response(HttpStatusCode = (int)HttpStatusCode.Forbidden, Description = FunctionDescription.Forbidden, ShowSchema = false)]
        [Display(Name = "Get", Description = "Ability to return a Routing Detail for the given Touchpoint.")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas/{touchpointID}")]HttpRequest theRequest,
            ILogger usingTraceWriter,
            string touchpointID,
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

            using (var inScope = await factory.BeginScopeFor(theRequest, usingTraceWriter))
            {
                return await adapter.GetAreaRoutingDetailFor(touchpointID, inScope);
            }
        }
    }
}