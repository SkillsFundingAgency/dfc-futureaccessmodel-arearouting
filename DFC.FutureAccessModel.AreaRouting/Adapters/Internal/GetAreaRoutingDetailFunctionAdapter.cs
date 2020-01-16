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

        public GetAreaRoutingDetailFunctionAdapter(
            IStoreAreaRoutingDetails storageProvider,
            IHttpResponseMessageHelper responseHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations)
        {
            StorageProvider = storageProvider;
            Respond = responseHelper;
            Faults = faultResponses;
            SafeOperations = safeOperations;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext useLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailFor(theTouchpointID, useLoggingScope),
                x => Faults.GetResponseFor(x, useLoggingScope));

        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the response message (success only)</returns>
        internal async Task<HttpResponseMessage> ProcessGetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext useLoggingScope)
        {
            await useLoggingScope.EnterMethod();

            It.IsEmpty(theTouchpointID)
                .AsGuard<MalformedRequestException>();

            var theDetail = await StorageProvider.GetAreaRoutingDetailFor(theTouchpointID);
            var withContent = JsonConvert.SerializeObject(theDetail);
            var response = Respond.Ok(withContent);

            await useLoggingScope.ExitMethod();

            return response;
        }

        public async Task<HttpResponseMessage> GetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext useLoggingScope) =>
            await SafeOperations.Try(
                () => ProcessGetAreaRoutingDetailBy(theLocation, useLoggingScope),
                x => Faults.GetResponseFor(x, useLoggingScope));

        internal async Task<HttpResponseMessage> ProcessGetAreaRoutingDetailBy(string theLocation, IScopeLoggingContext useLoggingScope)
        {
            await useLoggingScope.EnterMethod();

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

            await useLoggingScope.ExitMethod();

            return response;
        }
    }
}