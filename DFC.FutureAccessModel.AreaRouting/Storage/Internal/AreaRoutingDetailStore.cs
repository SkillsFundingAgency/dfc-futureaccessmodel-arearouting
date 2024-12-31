using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Storage.Internal
{
    /// <summary>
    /// the area routing detail store
    /// </summary>
    internal sealed class AreaRoutingDetailStore :
        IStoreAreaRoutingDetails
    {
        const string _partitionKey = "not_required";

        /// <summary>
        /// cosmos db provider
        /// </summary>
        public IWrapCosmosDbClient CosmosDbWrapper { get; }

        /// <summary>
        /// create an instance of hte <see cref="AreaRoutingDetailStore"/>
        /// </summary>
        /// <param name="paths">the storage paths (provider)</param>
        /// <param name="store">the document store</param>
        public AreaRoutingDetailStore(
            IWrapCosmosDbClient cosmosDbWrapper)
        {
            It.IsNull(cosmosDbWrapper)
                .AsGuard<ArgumentNullException>(nameof(cosmosDbWrapper));

            CosmosDbWrapper = cosmosDbWrapper;
        }

        /// <summary>
        /// get...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <returns>an area routing detail (the touchpoint)</returns>
        public async Task<IRoutingDetail> Get(string theTouchpoint)
        {
            var areaRoutingDetail = await CosmosDbWrapper.GetAreaRoutingDetailAsync(theTouchpoint);

            It.IsNull(areaRoutingDetail)
                .AsGuard<NoContentException>(theTouchpoint);

            return areaRoutingDetail;
        }

        /// <summary>
        /// add...
        /// </summary>
        /// <param name="theCandidate">the candidate (touchpoint)</param>
        /// <returns>the newly stored routing details (touchpoint)</returns>
        public async Task<IRoutingDetail> Add(IncomingRoutingDetail theCandidate)
        {
            It.IsNull(theCandidate)
                .AsGuard<ArgumentNullException>(nameof(theCandidate));

            var theTouchpoint = theCandidate?.TouchpointID;
            It.IsEmpty(theTouchpoint)
                .AsGuard<ArgumentNullException>(nameof(theTouchpoint));

            (await CosmosDbWrapper.AreaRoutingDetailExistsAsync(theTouchpoint, _partitionKey))
                .AsGuard<ConflictingResourceException>();

            var response = await CosmosDbWrapper.CreateAreaRoutingDetailAsync(theCandidate, _partitionKey);

            return response.Resource;
        }

        /// <summary>
        /// get all routing detail id's
        /// </summary>
        /// <returns>the full list of routing detail id's</returns>
        public async Task<IReadOnlyCollection<string>> GetAllIDs()
        {
            var result = await CosmosDbWrapper.GetAllAreaRoutingsAsync();

            return result.Select(x => x.TouchpointID).AsSafeReadOnlyList();
        }

        /// <summary>
        /// delete...
        /// </summary>
        /// <param name="theTouchpoint">the touchpoint (id)</param>
        /// <returns>an area routing detail (the touchpoint)</returns>
        public async Task Delete(string theTouchpoint)
        {
            It.IsEmpty(theTouchpoint)
                .AsGuard<ArgumentNullException>(nameof(theTouchpoint));

            (!await CosmosDbWrapper.AreaRoutingDetailExistsAsync(theTouchpoint, _partitionKey))
                .AsGuard<NoContentException>();

            await CosmosDbWrapper.DeleteAreaRoutingDetailAsync(theTouchpoint, _partitionKey);
        }
    }
}