﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DFC.Functions.DI.Standard.Attributes;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the api definition for 'swagger' document generation 
    /// </summary>
    public static class ApiDefinitionFunction
    {
        public const string ApiTitle = "Omni Channel Area Routing API";
        public const string ApiVersion = "1.0.0";
        public const string ApiDefinitionName = "api-definition";
        public const string ApiDescription =
            @"To support phone and email based request routing requirements between the NCS and the ABC's";

        /// <summary>
        /// run... (the api document generator function)
        /// </summary>
        /// <param name="theRequest">the http request</param>
        /// <param name="theDocumentGenerator">the document generator</param>
        /// <returns>a http response containing the generated document</returns>
        [FunctionName("ApiDefinition")]
        [Display(Name = "Get the API Definition", Description = @"Returns this swagger document")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas/api-definition")]HttpRequest theRequest,
            [Inject]ISwaggerDocumentGenerator theDocumentGenerator) =>
                await Task.Run(() =>
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
                        Assembly.GetExecutingAssembly(),
                        false, false); // don't include some irrelevant default parameters

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(theDocument)
                    };
                });
    }
}