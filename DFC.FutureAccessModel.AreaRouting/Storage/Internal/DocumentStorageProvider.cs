using System;
using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Documents;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    // TODO: work on this...
    internal sealed class DocumentStorageProvider :
        IProvideDocumentStorage
    {

        public IProvideStoragePaths StoragePaths { get; }

        public IStoreDocuments DocumentStore { get; }

        public DocumentStorageProvider(
            IProvideStoragePaths paths,
            IStoreDocuments store)
        {
            It.IsNull(paths)
                .AsGuard<ArgumentNullException>(nameof(paths));
            It.IsNull(store)
                .AsGuard<ArgumentNullException>(nameof(store));

            StoragePaths = paths;
            DocumentStore = store;
        }

        public async Task<IRoutingDetail> GetAreaRoutingDetail(string usingTouchpointID)
        {
            return await Task.FromResult(new RoutingDetail
            {
                TouchpointID = usingTouchpointID,
                Area = "Dummy Detail Region Name",
                EmailAddress = "test.address@education.gov.uk",
                SMSNumber = "07123456789",
                TelephoneNumber = "01234567890"
            });
        }

        public async Task<bool> DoesAreaRoutingDetailExistFor(string theTouchpointID)
        {
            var documentUri = StoragePaths.GetRoutingDetailResourcePathFor(theTouchpointID);

            try
            {
                var response = await DocumentStore.GetDocument<IRoutingDetail>(documentUri);
                return It.Has(response);
            }
            catch (DocumentClientException)
            {
                return false;
            }
        }
    }
}