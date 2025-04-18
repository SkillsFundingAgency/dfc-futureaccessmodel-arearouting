﻿using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.FutureAccessModel.AreaRouting.Validation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    /// <summary>
    /// get (the) area routing detail function adapter
    /// </summary>
    internal sealed class AreaRoutingDetailManagementFunctionAdapter :
        IManageAreaRoutingDetails
    {
        /// <summary>
        /// the fault (response provider)
        /// </summary>
        public IProvideFaultResponses Faults { get; }

        /// <summary>
        /// the safe operations (provider)
        /// </summary>
        public IProvideSafeOperations SafeOperations { get; }

        /// <summary>
        /// the (area) routing details (storage provider)
        /// </summary>
        public IStoreAreaRoutingDetails RoutingDetails { get; }

        /// <summary>
        /// i validate routing details
        /// </summary>
        public IValidateRoutingDetails RoutingDetail { get; }

        /// <summary>
        /// i analyse expressions
        /// </summary>
        public IAnalyseExpresssions Analyser { get; }

        /// <summary>
        /// i provide expression actions
        /// </summary>
        public IProvideExpressionActions Actions { get; }

        /// <summary>
        /// create an instance of <see cref="AreaRoutingDetailManagementFunctionAdapter"/>
        /// </summary>
        /// <param name="routingDetails">the storage provider</param>
        /// <param name="faultResponses">the fault responses (provider)</param>
        /// <param name="safeOperations">the safe operations (provider)</param>
        /// <param name="analyser">the expression analyser</param>
        /// <param name="actions">the expression action provider</param>
        public AreaRoutingDetailManagementFunctionAdapter(
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations,
            IStoreAreaRoutingDetails routingDetails,
            IValidateRoutingDetails validator,
            IAnalyseExpresssions analyser,
            IProvideExpressionActions actions)
        {
            It.IsNull(faultResponses)
                .AsGuard<ArgumentNullException>(nameof(faultResponses));
            It.IsNull(safeOperations)
                .AsGuard<ArgumentNullException>(nameof(safeOperations));
            It.IsNull(routingDetails)
                .AsGuard<ArgumentNullException>(nameof(routingDetails));
            It.IsNull(validator)
                .AsGuard<ArgumentNullException>(nameof(validator));
            It.IsNull(analyser)
                .AsGuard<ArgumentNullException>(nameof(analyser));
            It.IsNull(actions)
                .AsGuard<ArgumentNullException>(nameof(actions));

            Faults = faultResponses;
            SafeOperations = safeOperations;
            RoutingDetails = routingDetails;
            RoutingDetail = validator;
            Analyser = analyser;
            Actions = actions;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<IActionResult> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inScope)
        {
            return await SafeOperations.Try(() => ProcessGetAreaRoutingDetailFor(theTouchpointID, inScope), x => Faults.GetResponseFor(x, TypeOfFunction.GetByID, inScope));
        }

        /// <summary>
        /// process, get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success only)</returns>
        internal async Task<IActionResult> ProcessGetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            It.IsEmpty(theTouchpointID)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"seeking the routing details: '{theTouchpointID}'");

            var theDetail = await RoutingDetails.Get(theTouchpointID);

            It.IsNull(theDetail)
                .AsGuard<MalformedRequestException>(theTouchpointID);

            await inScope.Information($"candidate search complete: '{theDetail.TouchpointID}'");
            await inScope.Information($"preparing response...");

            var response = new OkObjectResult(theDetail);

            await inScope.Information($"preparation complete...");
            await inScope.ExitMethod();

            return response;
        }

        /// <summary>
        /// get (the) area routing detail by...
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<IActionResult> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inScope)
        {
            return await SafeOperations.Try(() => ProcessGetAreaRoutingDetailBy(theLocation, inScope), x => Faults.GetResponseFor(x, TypeOfFunction.GetByLocation, inScope));
        }

        /// <summary>
        /// process, get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        internal async Task<IActionResult> ProcessGetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            theLocation = theLocation?.Trim();

            It.IsEmpty(theLocation)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"seeking the routing details for: '{theLocation}'");
            await inScope.Information($"analysing the expression type...");

            var theExpressionType = Analyser.GetTypeOfExpressionFor(theLocation);

            await inScope.Information($"seeking the action for expression type: '{theExpressionType}'");

            var actionDo = Actions.GetActionFor(theExpressionType);

            await inScope.Information($"action for expression type: '{actionDo.Method.Name}'");

            var theTouchpoint = await actionDo(theLocation, inScope);

            await inScope.Information($"action execution complete...");
            await inScope.ExitMethod();

            return await ProcessGetAreaRoutingDetailFor(theTouchpoint, inScope);
        }

        /// <summary>
        /// add new area routing detail
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<IActionResult> AddAreaRoutingDetailUsing(string theContent, IScopeLoggingContext inScope)
        {
            return await SafeOperations.Try(() => ProcessAddAreaRoutingDetailUsing(theContent, inScope), x => Faults.GetResponseFor(x, TypeOfFunction.Post, inScope));
        }

        /// <summary>
        /// process, add new area routing detail
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<IActionResult> ProcessAddAreaRoutingDetailUsing(
            string theContent,
            IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            It.IsEmpty(theContent)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"deserialising the submitted content: '{theContent}'");

            var theDeserialized = JsonConvert.DeserializeObject<PostedRoutingDetail>(theContent);

            await inScope.Information("deserialisation complete...");

            await inScope.Information("mapping the deserialised content...");

            It.IsNull(theDeserialized)
                .AsGuard<MalformedRequestException>();

            var theCandidate = new IncomingRoutingDetail
            {
                TouchpointID = theDeserialized.TouchpointID,
                Area = theDeserialized.Area,
                TelephoneNumber = theDeserialized.TelephoneNumber,
                SMSNumber = theDeserialized.SMSNumber,
                EmailAddress = theDeserialized.EmailAddress,
            };

            await inScope.Information("mapping complete...");

            It.IsNull(theCandidate)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"validating the candidate: '{theCandidate.TouchpointID}'");

            await RoutingDetail.Validate(theCandidate);

            await inScope.Information($"validation complete...");
            await inScope.Information($"adding the candidate: '{theCandidate.TouchpointID}'");

            var result = await RoutingDetails.Add(theCandidate);

            await inScope.Information($"candidate addition complete...");
            await inScope.Information($"preparing response...");

            var response = new JsonResult(result, new JsonSerializerOptions())
            {
                StatusCode = (int)HttpStatusCode.Created
            };

            await inScope.Information($"preparation complete...");
            await inScope.ExitMethod();

            return response;
        }

        /// <summary>
        /// get all route id's
        /// </summary>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<IActionResult> GetAllRouteIDs(IScopeLoggingContext inScope)
        {
            return await SafeOperations.Try(() => ProcessGetAllRouteIDs(inScope), x => Faults.GetResponseFor(x, TypeOfFunction.GetAll, inScope));
        }

        /// <summary>
        /// process, get all route id's
        /// </summary>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success only)</returns>
        internal async Task<IActionResult> ProcessGetAllRouteIDs(IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();
            await inScope.Information("seeking all routing ids");

            var result = await RoutingDetails.GetAllIDs();

            await inScope.Information($"found {result.Count} record(s)...");

            var theCandidate = $"{{ [{string.Join(", ", result.Select(x => $"\"{x}\""))}] }}";

            await inScope.Information($"candidate content: '{theCandidate}'");
            await inScope.Information($"preparing response...");

            var response = new OkObjectResult(theCandidate);

            await inScope.Information($"preparation complete...");
            await inScope.ExitMethod();

            return response;
        }

        /// <summary>
        /// delete an area routing detail using...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<IActionResult> DeleteAreaRoutingDetailUsing(string theTouchpointID, IScopeLoggingContext inScope)
        {
            return await SafeOperations.Try(() => ProcessDeleteAreaRoutingDetailUsing(theTouchpointID, inScope), x => Faults.GetResponseFor(x, TypeOfFunction.Delete, inScope));
        }

        /// <summary>
        /// process, delete an area routing detail using...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        internal async Task<IActionResult> ProcessDeleteAreaRoutingDetailUsing(string theTouchpointID, IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();
            await inScope.Information($"deleting the routing details for '{theTouchpointID}'");

            await RoutingDetails.Delete(theTouchpointID);

            await inScope.Information($"preparing response...");

            var response = new OkResult();

            await inScope.Information($"preparation complete...");
            await inScope.ExitMethod();

            return response;
        }
    }
}