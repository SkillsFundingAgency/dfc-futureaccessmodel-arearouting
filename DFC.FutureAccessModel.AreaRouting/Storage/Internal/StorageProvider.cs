using System.Threading.Tasks;
using DFC.FutureAccessModel.AreaRouting.Models;
using Microsoft.Azure.Documents;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    // TODO: work on this...
    internal class StorageProvider :
        IProvideStorageAccess
    {
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
            var documentUri = StoragePath.GetRoutingDetailResourcePathFor(theTouchpointID);

            var client = DocumentDB.Client;

            try
            {
                var response = await client.ReadDocumentAsync(documentUri);
                return response?.Resource != null;
            }
            catch (DocumentClientException)
            {
                return false;
            }
        }
    }
}