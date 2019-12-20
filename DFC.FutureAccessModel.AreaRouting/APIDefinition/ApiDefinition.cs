using DFC.Functions.DI.Standard.Attributes;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace DFC.FutureAccessModel.AreaRouting.APIDefinition
{
    /// <summary>
    /// the api definition for 'swagger' generation 
    /// </summary>
    public static class ApiDefinition
    {
        public const string ApiTitle = "AreaRouting";
        public const string ApiVersion = "1.0.0";
        public const string ApiDefinitionName = "API-Definition";
        public const string ApiDefRoute = ApiTitle + "/" + ApiDefinitionName; // TODO: do we need this?
        public const string ApiDescription =
            @"To support email routing requirements between DSS and the ABC's. Plus lot's of
            other stuff I don't know yet as I don't have access to the specification";

        /// <summary>
        /// run... (the api document generator function)
        /// </summary>
        /// <param name="theRequest">the http request</param>
        /// <param name="theDocumentGenerator">the document generator</param>
        /// <returns>a http response containing the generated document</returns>
        [FunctionName(ApiDefinitionName)]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiDefRoute)]HttpRequest theRequest,
            [Inject]ISwaggerDocumentGenerator theDocumentGenerator)
        {
            It.IsNull(theRequest)
                .AsGuard<ArgumentNullException>(nameof(theRequest));
            It.IsNull(theDocumentGenerator)
                .AsGuard<ArgumentNullException>(nameof(theDocumentGenerator));

            var theDocument = theDocumentGenerator.GenerateSwaggerDocument(
                theRequest,
                ApiTitle,
                ApiDescription,
                ApiDefinitionName,
                ApiVersion,
                Assembly.GetExecutingAssembly());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(theDocument)
            };
        }
    }
}