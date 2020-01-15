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
        public IStoreAreaRoutingDetails StorageProvider { get; }

        public IProvideFaultResponses Faults { get; }

        public IProvideSafeOperations SafeOperations { get; }

        public IHttpResponseMessageHelper ResponseHelper { get; }

        public GetAreaRoutingDetailFunctionAdapter(
            IStoreAreaRoutingDetails storageProvider,
            IHttpResponseMessageHelper httpResponseMessageHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations)
        {
            StorageProvider = storageProvider;
            ResponseHelper = httpResponseMessageHelper;
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

            var result = await StorageProvider.GetAreaRoutingDetailFor(theTouchpointID);
            var contentResult = JsonConvert.SerializeObject(result);
            var response = ResponseHelper.Ok(contentResult);

            await useLoggingScope.ExitMethod();

            return await Task.FromResult(response);
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

            var result = await StorageProvider.GetAreaRoutingDetailFor(theLocation);
            var contentResult = JsonConvert.SerializeObject(result);
            var response = ResponseHelper.Ok(contentResult);

            await useLoggingScope.ExitMethod();

            return await Task.FromResult(response);
        }
    }
}