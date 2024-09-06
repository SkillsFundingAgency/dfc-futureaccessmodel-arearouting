using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.Swagger.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Functions
{
    /// <summary>
    /// the api definition for 'swagger' document generation 
    /// </summary>
    public sealed class ApiDefinitionFunction
    {
        public const string ApiTitle = "Omni Channel Area Routing API";
        public const string ApiVersion = "1.0.0";
        public const string ApiDefinitionName = "api-definition";
        public const string ApiDescription =
            @"To support phone and email based request routing requirements between the NCS and the ABC's";

        /// <summary>
        /// (the swagger document) generator
        /// </summary>
        public ISwaggerDocumentGenerator Generator { get; }

        /// <summary>
        /// initialise an instance of the <see cref="ApiDefinitionFunction"/>
        /// </summary>
        /// <param name="generator">(the swagger document) generator</param>
        public ApiDefinitionFunction(ISwaggerDocumentGenerator generator)
        {
            It.IsNull(generator)
                .AsGuard<ArgumentNullException>(nameof(generator));

            Generator = generator;
        }

        /// <summary>
        /// run... (the api document generator function)
        /// </summary>
        /// <param name="request">the http request</param>
        /// <returns>a http response containing the generated document</returns>
        [Function("ApiDefinition")]
        [Display(Name = "Get the API Definition", Description = @"Returns this swagger document")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "areas/api-definition")]HttpRequest request) =>
            await Task.Run<IActionResult>(() =>
            {
                It.IsNull(request)
                    .AsGuard<ArgumentNullException>(nameof(request));

                var document = Generator.GenerateSwaggerDocument(
                    request,
                    ApiTitle,
                    ApiDescription,
                    ApiDefinitionName,
                    ApiVersion,
                    Assembly.GetExecutingAssembly(),
                    false, false); // don't include some irrelevant default parameters

                if (string.IsNullOrEmpty(document))
                    return new NoContentResult();

                return new OkObjectResult(document);
            });
    }
}