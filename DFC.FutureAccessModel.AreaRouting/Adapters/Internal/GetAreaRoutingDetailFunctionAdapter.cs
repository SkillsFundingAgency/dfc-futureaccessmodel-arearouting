using System;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.HTTP.Standard;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    /// <summary>
    /// get (the) area routing detail function adapter
    /// </summary>
    internal sealed class GetAreaRoutingDetailFunctionAdapter :
        IGetAreaRoutingDetails
    {
        /// <summary>
        /// the (area routing) storage provider
        /// </summary>
        public IStoreAreaRoutingDetails StorageProvider { get; }

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
        /// create an instance of <see cref="GetAreaRoutingDetailFunctionAdapter"/>
        /// </summary>
        /// <param name="storageProvider">the storage provider</param>
        /// <param name="responseHelper">the response helper</param>
        /// <param name="faultResponses">the fault responses (provider)</param>
        /// <param name="safeOperations">the safe operations (provider)</param>
        public GetAreaRoutingDetailFunctionAdapter(
            IStoreAreaRoutingDetails storageProvider,
            IHttpResponseMessageHelper responseHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations)
        {
            It.IsNull(storageProvider)
                .AsGuard<ArgumentNullException>(nameof(storageProvider));
            It.IsNull(responseHelper)
                .AsGuard<ArgumentNullException>(nameof(responseHelper));
            It.IsNull(faultResponses)
                .AsGuard<ArgumentNullException>(nameof(faultResponses));
            It.IsNull(safeOperations)
                .AsGuard<ArgumentNullException>(nameof(safeOperations));

            StorageProvider = storageProvider;
            Respond = responseHelper;
            Faults = faultResponses;
            SafeOperations = safeOperations;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext inLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailFor(theTouchpointID, inLoggingScope),
                x => Faults.GetResponseFor(x, inLoggingScope));

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

            var theDetail = await StorageProvider.GetAreaRoutingDetailFor(theTouchpointID);
            var withContent = JsonConvert.SerializeObject(theDetail);
            var response = Respond.Ok(withContent);

            await inLoggingScope.ExitMethod();

            return response;
        }

        /// <summary>
        /// get (the) area routing detail by...
        /// </summary>
        /// <param name="theLocation">the location</param>
        /// <param name="inLoggingScope">in logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<HttpResponseMessage> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext inLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailBy(theLocation, inLoggingScope),
                x => Faults.GetResponseFor(x, inLoggingScope));

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

            // TODO: things...
            // integrate postcodes io
            //      simplify code / interface ?
            // determine what 'the location' is
            //      postcode ?
            //      town ?
            //      something else ?
            // define
            //      the local authority model
            //      the storage provider
            //      extend the storage path provider
            // fall back process
            //      redirect to the national centre?

            var theDetail = await StorageProvider.GetAreaRoutingDetailFor(theLocation);
            var withContent = JsonConvert.SerializeObject(theDetail);
            var response = Respond.Ok(withContent);

            await inLoggingScope.ExitMethod();

            return response;
        }
    }
}