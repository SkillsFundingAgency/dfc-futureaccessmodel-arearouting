using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.HTTP.Standard;
using Newtonsoft.Json;

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
        /// the response (helper)
        /// </summary>
        public IHttpResponseMessageHelper Respond { get; }

        /// <summary>
        /// the (area) routing details (storage provider)
        /// </summary>
        public IStoreAreaRoutingDetails RoutingDetails { get; }

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
        /// <param name="responseHelper">the response helper</param>
        /// <param name="faultResponses">the fault responses (provider)</param>
        /// <param name="safeOperations">the safe operations (provider)</param>
        /// <param name="analyser">the expression analyser</param>
        /// <param name="actions">the expression action provider</param>
        public AreaRoutingDetailManagementFunctionAdapter(
            IHttpResponseMessageHelper responseHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations,
            IStoreAreaRoutingDetails routingDetails,
            IAnalyseExpresssions analyser,
            IProvideExpressionActions actions)
        {
            It.IsNull(responseHelper)
                .AsGuard<ArgumentNullException>(nameof(responseHelper));
            It.IsNull(faultResponses)
                .AsGuard<ArgumentNullException>(nameof(faultResponses));
            It.IsNull(safeOperations)
                .AsGuard<ArgumentNullException>(nameof(safeOperations));
            It.IsNull(routingDetails)
                .AsGuard<ArgumentNullException>(nameof(routingDetails));
            It.IsNull(analyser)
                .AsGuard<ArgumentNullException>(nameof(analyser));
            It.IsNull(actions)
                .AsGuard<ArgumentNullException>(nameof(actions));

            Respond = responseHelper;
            Faults = faultResponses;
            SafeOperations = safeOperations;
            RoutingDetails = routingDetails;
            Analyser = analyser;
            Actions = actions;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        [ExcludeFromCodeCoverage]
        public async Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailFor(theTouchpointID, inLoggingScope),
                x => Faults.GetResponseFor(x, TypeofMethod.Get, inLoggingScope));

        /// <summary>
        /// process, get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success only)</returns>
        internal async Task<HttpResponseMessage> ProcessGetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inLoggingScope)
        {
            await inLoggingScope.EnterMethod();

            It.IsEmpty(theTouchpointID)
                .AsGuard<MalformedRequestException>();

            var theDetail = await RoutingDetails.Get(theTouchpointID);
            var withContent = JsonConvert.SerializeObject(theDetail);
            var response = Respond.Ok(withContent);

            await inLoggingScope.ExitMethod();

            return response;
        }

        /// <summary>
        /// get (the) area routing detail by...
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        [ExcludeFromCodeCoverage]
        public async Task<HttpResponseMessage> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailBy(theLocation, inLoggingScope),
                x => Faults.GetResponseFor(x, TypeofMethod.Get, inLoggingScope));

        /// <summary>
        /// process, get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        internal async Task<HttpResponseMessage> ProcessGetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inLoggingScope)
        {
            await inLoggingScope.EnterMethod();

            It.IsEmpty(theLocation)
                .AsGuard<MalformedRequestException>();

            var theExpressionType = Analyser.GetTypeOfExpressionFor(theLocation);
            var actionDo = Actions.GetActionFor(theExpressionType);
            var theTouchpoint = await actionDo(theLocation, inLoggingScope);

            await inLoggingScope.ExitMethod();

            return await ProcessGetAreaRoutingDetailFor(theTouchpoint, inLoggingScope);
        }

        /// <summary>
        /// add new area routing detail
        /// excluded from coverage as moq doesn't support the lambda complexity for this routine
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        [ExcludeFromCodeCoverage]
        public async Task<HttpResponseMessage> AddAreaRoutingDetailUsing(
            string theContent,
            IScopeLoggingContext inScope) =>
            await SafeOperations.Try(
                () => ProcessAddAreaRoutingDetailUsing(theContent, inScope),
                x => Faults.GetResponseFor(x, TypeofMethod.Post, inScope));

        /// <summary>
        /// process, add new area routing detail
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint</param>
        /// <param name="usingContent">using content</param>
        /// <param name="inScope">in scope</param>
        /// <returns>the result of the operation</returns>
        public async Task<HttpResponseMessage> ProcessAddAreaRoutingDetailUsing(
            string theContent,
            IScopeLoggingContext inScope)
        {
            await inScope.EnterMethod();

            It.IsEmpty(theContent)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"deserialising the submitted content: {theContent}");

            var theCandidate = JsonConvert.DeserializeObject<RoutingDetail>(theContent);

            await inScope.Information("deserialisation complete...");

            It.IsEmpty(theCandidate?.TouchpointID)
                .AsGuard<MalformedRequestException>();

            await inScope.Information($"adding the area routing candidate: {theCandidate?.TouchpointID}");

            var result = await RoutingDetails.Add(theCandidate);

            await inScope.Information($"candidate addition complete...");

            await inScope.ExitMethod();

            return Respond.Created().SetContent(result);
        }
    }
}