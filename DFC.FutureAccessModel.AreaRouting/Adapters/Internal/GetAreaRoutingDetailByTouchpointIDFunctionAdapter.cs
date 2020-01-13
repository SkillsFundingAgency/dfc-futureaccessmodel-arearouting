using System.Net.Http;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Providers;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.HTTP.Standard;
using DFC.JSON.Standard;

namespace DFC.FutureAccessModel.AreaRouting.Adapters.Internal
{
    internal sealed class GetAreaRoutingDetailByTouchpointIDFunctionAdapter :
        IGetAreaRoutingDetailByTouchpointID
    {
        public IProvideDocumentStorage StorageProvider { get; }
        public IProvideFaultResponses Faults { get; }
        public IProvideSafeOperations SafeOperations { get; }
        public IHttpResponseMessageHelper ResponseHelper { get; }

        public IJsonHelper JsonHelper { get; }

        public GetAreaRoutingDetailByTouchpointIDFunctionAdapter(
            IProvideDocumentStorage storageProvider,
            IHttpResponseMessageHelper httpResponseMessageHelper,
            IProvideFaultResponses faultResponses,
            IProvideSafeOperations safeOperations,
            IJsonHelper jsonHelper)
        {
            StorageProvider = storageProvider;
            ResponseHelper = httpResponseMessageHelper;
            Faults = faultResponses;
            SafeOperations = safeOperations;
            JsonHelper = jsonHelper;
        }

        /// <summary>
        /// get (the) area routing detail for...
        /// </summary>
        /// <param name="theTouchpointID">the touchpoint id</param>
        /// <param name="useLoggingScope">use (the) logging scope</param>
        /// <returns>the currently running task containing the response message (success or fail)</returns>
        public async Task<HttpResponseMessage> GetAreaRoutingDetailFor(string theTouchpointID, IScopeLoggingContext useLoggingScope) =>
            await SafeOperations.Try(() => ProcessGetAreaRoutingDetailFor(theTouchpointID, useLoggingScope), x => Faults.GetResponseFor(x, useLoggingScope));

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

            var result = await StorageProvider.GetAreaRoutingDetail(theTouchpointID);
            /*
            var correlationId = httpRequestHelper.GetDssCorrelationId(req);
            if (string.IsNullOrEmpty(correlationId))
                log.LogInformation("Unable to locate 'DssCorrelationId' in request header");

            if (!Guid.TryParse(correlationId, out var correlationGuid))
            {
                log.LogInformation("Unable to parse 'DssCorrelationId' to a Guid");
                correlationGuid = Guid.NewGuid();
            }

            var touchpointId = httpRequestHelper.GetDssTouchpointId(req);
            if (string.IsNullOrEmpty(touchpointId))
            {
                logHelper.LogInformationMessage(log, correlationGuid, "Unable to locate 'TouchpointId' in request header");
                return httpResponseMessageHelper.BadRequest();
            }

            logHelper.LogInformationMessage(log, correlationGuid,
                $"Get Action Plan C# HTTP trigger function  processed a request. By Touchpoint: {touchPointID}");

            if (!Guid.TryParse(customerId, out var customerGuid))
            {
                logHelper.LogInformationMessage(log, correlationGuid, string.Format("Unable to parse 'customerId' to a Guid: {0}", customerId));
                return httpResponseMessageHelper.BadRequest(customerGuid);
            }

            logHelper.LogInformationMessage(log, correlationGuid, string.Format("Attempting to see if customer exists {0}", customerGuid));
            var doesCustomerExist = await resourceHelper.DoesCustomerExist(customerGuid);

            if (!doesCustomerExist)
            {
                logHelper.LogInformationMessage(log, correlationGuid, string.Format("Customer does not exist {0}", customerGuid));
                return httpResponseMessageHelper.NoContent(customerGuid);
            }

            logHelper.LogInformationMessage(log, correlationGuid, string.Format("Attempting to get action plan for customer {0}", customerGuid));
            var actionPlans = await actionPlanGetService.GetActionPlansAsync(customerGuid);

            return actionPlans == null ?
                httpResponseMessageHelper.NoContent(customerGuid) :
                httpResponseMessageHelper.Ok(jsonHelper.SerializeObjectsAndRenameIdProperty(actionPlans, "id", "ActionPlanId"));
                */

            var contentResult = JsonHelper.SerializeObjectAndRenameIdProperty(default(RoutingDetail), "id", "TouchpointID");
            var response = ResponseHelper.Ok(contentResult);
            return await Task.FromResult(response);
        }
    }
}